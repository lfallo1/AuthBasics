using System.Threading.Tasks;
using AuthenticationBasics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace AuthenticationBasics.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signinManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
