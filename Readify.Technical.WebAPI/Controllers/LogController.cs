using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        [HttpGet]

        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await GetLogFileContent());
            }
            catch (Exception ex) { return BadRequest(); }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile()
        {
            await DeleteLogFile();
            return Ok();
        }

        private async Task<object> GetLogFileContent()
        {
            string path = System.Environment.CurrentDirectory + "\\log";
            string filename = @"\requestlog.txt";
            using (StreamReader reader = new StreamReader(path+filename))
            {            
            return reader.ReadToEnd();
            }
        }
        private async Task DeleteLogFile()
        {
            string path = System.Environment.CurrentDirectory + "\\log";
            string filename = @"\requestlog.txt";
            if(System.IO.File.Exists(path + @"\requestlog.txt"))
            {
                System.IO.File.Delete(path + @"\requestlog.txt");
            }
        }
    }
}
