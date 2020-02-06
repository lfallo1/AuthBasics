using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationBasics.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecretInfo()
        {
            return Ok(new { response = "Private data" });
        }
    }
}
