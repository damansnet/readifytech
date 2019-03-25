using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.IO;
using Newtonsoft.Json;

namespace Readify.Technical.WebAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly Action<RequestProfilerModel> _requestResponseHandler;
        private const int ReadChunkBufferLength = 4096;

        public RequestResponseLoggingMiddleware(RequestDelegate next, Action<RequestProfilerModel> requestResponseHandler)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _requestResponseHandler = requestResponseHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var model = new RequestProfilerModel
            {
                RequestTime = new DateTimeOffset(),
                Context = context,
                Request = await FormatRequest(context)
            };

            Stream originalBody = context.Response.Body;

            using (MemoryStream newResponseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = newResponseBody;

                await _next(context);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalBody);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                model.Response = FormatResponse(context, newResponseBody);
                model.ResponseTime = new DateTimeOffset();
                _requestResponseHandler(model);
            }
        }

        private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            var obj = new
            {
                Schema = request.Scheme,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString,
                Status = response.StatusCode,
                Response = ReadStreamInChunks(newResponseBody)
            };

            return JsonConvert.SerializeObject(obj);
        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            var obj = new
            {
                Schema = request.Scheme,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString,
                Response = await GetRequestBody(request)
            };
           return JsonConvert.SerializeObject(obj);
        }

        public async Task<string> GetRequestBody(HttpRequest request)
        {
             
            request.EnableRewind();
            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await request.Body.CopyToAsync(requestStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                return ReadStreamInChunks(requestStream);
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;
                //do while: is useful for the last iteration in case readChunkLength < chunkLength
                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

        public class RequestProfilerModel
        {
            public DateTimeOffset RequestTime { get; set; }
            public HttpContext Context { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public DateTimeOffset ResponseTime { get; set; }
        }
    }
}
