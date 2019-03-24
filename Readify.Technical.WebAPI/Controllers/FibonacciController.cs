using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class FibonacciController : Controller
    {
        [HttpGet]
        
        public async Task<ActionResult> Get([FromQuery]int  n)
        {
            try
            {
                return Ok( await GetFibonacciNumber(n));
            }catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return BadRequest();

            }
        }

        private async Task<long> GetFibonacciNumber(long num)
        {
            try { 
            long a = 0;
            long b = 1;
          
            for (long i = 0; i < num; i++)
            {
                long temp = a;
                a = b;
                b = temp + b;
            }
            return (a>0)? a : throw new Exception("negative number ß");
            } catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
