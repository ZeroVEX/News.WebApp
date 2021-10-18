using Logging;
using NewsApp.DomainModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.NewsServices;
using NewsApp.Foundation.UsersServices;
using NewsApp.Foundation.UsersServices.Validators;
using NewsApp.Repositories;
using NewsApp.Repositories.Contexts;
using NewsApp.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NewsApp.WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");

            services.AddControllersWithViews();
            services.AddDbContext<NewsDbContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = User.PasswordMinLength;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddPasswordValidator<NewsPasswordValidator>()
                .AddUserValidator<NewsUserValidator>()
                .AddClaimsPrincipalFactory<NewsClaimPrincipalFactory>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<INewsManagementService, NewsManagementService>();
            services.AddScoped<INewsUnitOfWork, NewsUnitOfWork>();
            services.AddScoped<ILog, LoggerAdapter>();
            services.AddScoped(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                return loggerFactory.CreateLogger("Logging");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Registration}/{action=Register}/{id?}");
            });
        }
    }
}