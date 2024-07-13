using Microsoft.AspNetCore.Mvc;

namespace ASP_Custom_Routing_Constraint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController : Controller
    {
        [HttpGet]
        [Route("get/{number:int:even}")]
        public IActionResult GetEven(int number)
        {
            return Ok($"Number {number} is even");
        }

        [HttpGet]
        [Route("get/{number:int:positiveOddDouble}")]
        public IActionResult Get(int number)
        {
            return Ok($"Number {number} is positive, odd and double");
        }
    }
}
