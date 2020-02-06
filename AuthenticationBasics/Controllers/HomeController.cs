using Microsoft.AspNetCore.Mvc;

namespace AuthenticationBasics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHome()
        {
            return Ok(new { response = "Public data" });
        }
    }
}
