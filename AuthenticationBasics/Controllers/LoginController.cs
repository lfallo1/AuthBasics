using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationBasics.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;

        public LoginController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            return await _Login(username, password);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        private async Task<IActionResult> _Login(string username, string password)
        {
            var signinResult = await _signinManager.PasswordSignInAsync(username, password, false, false);

            if (signinResult.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }

            return View();
        }
    }
}
