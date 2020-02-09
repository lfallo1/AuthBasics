using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace AuthenticationBasics.Controllers
{
    public class RegisterController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IEmailService _emailService;

        public RegisterController(UserManager<IdentityUser> userManager,
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

        public IActionResult RegristrationPendingVerification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string email, string phone, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = email,
                PhoneNumber = phone
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Register", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync(email, "Confirm email", $"<a href=\"{link}\" target=\"_blank\">Click to Verfy Email</a>", true);

                //generate email token
                return RedirectToAction("RegristrationPendingVerification");
            }
            return View("Register");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (null != user)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return View();
                }
            }
            return BadRequest();
        }
    }
}
