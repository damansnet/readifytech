using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class FibonacciController : Controller
    {
        [HttpGet]
        
        public async Task<ActionResult> Get([FromQuery]long  n)
        {
            try
            {
                
                    return Ok(await GetFibonacciNumber(n));
                

            }catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return BadRequest();

            }
        }

        private async Task<long> GetFibonacciNumber(long num)
        {
            try {

                bool isNegative = num < 0;

                double phi = (1 + Math.Sqrt(5)) / 2;
                long result = (long)Math.Round(Math.Pow(phi, Math.Abs(num))
                                        / Math.Sqrt(5));
                if (result < 0)
                    throw new Exception("error");
                return  isNegative ? result *-1 : result ;

                
            } catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
