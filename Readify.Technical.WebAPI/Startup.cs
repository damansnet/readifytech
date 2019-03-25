using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Readify.Technical.WebAPI.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using static Readify.Technical.WebAPI.Middleware.RequestResponseLoggingMiddleware;

namespace Readify.Technical.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ReadifyTestAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadifyTestAPI");
            });
            Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
            {

                 LogToFile(requestProfilerModel.Request + ":{" + requestProfilerModel.RequestTime + "}  Response:" + requestProfilerModel.Response);

            };
            app.UseMiddleware<RequestResponseLoggingMiddleware>(requestResponseHandler);
            app.UseStaticFiles();
            app.UseMvc();
        }
        private async Task LogToFile(string message)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(message);
            string path = System.Environment.CurrentDirectory + "\\log";
            if(!File.Exists(path+@"\requestlog.txt"))
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

            }
           
            using (FileStream fs = new FileStream(path+@"\requestlog.txt",FileMode.Append))
            {
                await fs.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }
    }
}
