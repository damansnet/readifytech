using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Readify.Technical.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TriangleTypeController:Controller
    {

       
        [HttpGet]
        public IActionResult Get([FromQuery]Int64 a, [FromQuery]Int64 b, [FromQuery] Int64 c)
        {
            if(!this.ModelState.IsValid)
            {
                return BadRequest(new { message = "Request is invalid" });
            }


            try {
                 
            
            return Ok( validateResult(GetTriangleType(a, b, c)));
            } catch(Exception ex)
            {
                return BadRequest();

            }
        }

        private string validateResult(Triangle triangle)
        {
            switch (triangle)
            {
                case Triangle.Equilateral:
                    return "Equilateral";
                case Triangle.Isosceles:
                    return "Isosceles";
                case Triangle.Scalene:
                    return "Scalene";
                case Triangle.Error:
                    return "Error";
            }
            return "Error";
        }

        public enum Triangle
        {
            Scalene = 1, // all sides are different
            Isosceles = 2, // 2 sides are equal
            Equilateral = 3, // all sides equal
            Error = 4 // error
        }

        private Triangle GetTriangleType(Int64 side1, Int64 side2, Int64 side3)
        {
            try
            {
                //checked if any side of triagle is not zero
                if (side1 <= 0 || side2 <= 0 || side3 <= 0)
                {
                    return Triangle.Error;
                }

                //check for triangle inequality theorem

                if (((side2 + side3) < side1) || ((side1 + side3) < side2) || ((side1 + side2) < side3))

                {
                    return Triangle.Error;

                }

                //check if all sides are equal
                if (side1 == side2 && side1 == side3)
                {
                    return Triangle.Equilateral;
                }
                else if (side1 == side2 || side1 == side3 || side2 == side3) //check if any two sides are equal
                {
                    return Triangle.Isosceles;
                }
                else //this block will be executed of none of the sides are equal
                {
                    return Triangle.Scalene;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
