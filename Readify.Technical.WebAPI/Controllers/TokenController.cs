using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            return "adc73c23-b56b-4e6e-b9bf-33f13858eeb7";
        }

        
        
       
    }
}
