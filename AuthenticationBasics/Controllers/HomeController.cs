using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationBasics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHome()
        {
            return Ok(new { response = "Data anyone can see" });
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecretInfo()
        {
            return Ok(new { response = "Private data" });
        }

        [HttpGet("authenticate")]
        public IActionResult Authenticate()
        {
            var userClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "lfallon"),
                new Claim(ClaimTypes.Email, "lfallon@something.org")
            };

            var licenseClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "lance fallon"),
                new Claim("LicenseNumber", "340874074502702075047")
            };

            var userIdentity = new ClaimsIdentity(userClaim, "userIdentity");
            var licenseIdentity = new ClaimsIdentity(licenseClaim, "licenseIdentity");

            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            //Response.Cookies.Append("Auth.Cookie", "Password");
            return Ok();
        }
    }
}
