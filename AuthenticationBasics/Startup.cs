using AuthenticationBasics.AuthorizationPolicies;
using AuthenticationBasics.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace AuthenticationBasics
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("AuthBasicsDb");
                //opt.UseSqlServer("Server=LAPTOP-733A049A;Database=IdentityAuth;Trusted_Connection=true;");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                    //config.Password.RequiredLength = 10;
                    //config.Password.RequireDigit = true;
                    //config.Lockout.MaxFailedAccessAttempts = 10;
                    //config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //    .RequireAuthenticatedUser()
                //    .Build();
                //config.DefaultPolicy = defaultAuthPolicy;

                /* Add custom policy */
                config.AddPolicy("Domain", policyBuilder =>
                {
                    policyBuilder.RequireCustomDomain("sbgtv.com");
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireDomainHandler>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Auth.Cookie";
                config.LoginPath = "/Login";
            });

            services.AddMailKit(config => config.UseMailKit(_config.GetSection("Email").Get<MailKitOptions>()));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //check valid user
            app.UseAuthentication();

            //check if authenticcated user is allowed access
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
