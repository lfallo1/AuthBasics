using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationBasics.AuthorizationPolicies
{
    public class CustomRequireDomain : IAuthorizationRequirement
    {
        public CustomRequireDomain(string domain)
        {
            Domain = domain;
        }

        public string Domain { get; }
    }

    public class CustomRequireDomainHandler : AuthorizationHandler<CustomRequireDomain>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CustomRequireDomainHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomRequireDomain requirement)
        {
            var user = _userManager.Users.First(u => u.UserName == context.User.Identity.Name);
            if (user.Email.EndsWith(requirement.Domain))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomDomain(this AuthorizationPolicyBuilder builder, string domain)
        {
            builder.AddRequirements(new CustomRequireDomain(domain));
            return builder;
        }
    }
}
