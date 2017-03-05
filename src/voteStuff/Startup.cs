using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using voteStuff.Entities;
using voteStuff.Services;

namespace voteStuff
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IVotingService, VotingService>();
            services.AddDbContext<VoteDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("VotesDbCon")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<VoteDbContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseIdentity();
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"],
                Scope = { "public_profile" },
                Fields = { "picture" },
                SaveTokens = true
            });
            app.UseMvc(RouterConfiguration);
        }

        private void RouterConfiguration(IRouteBuilder obj)
        {
            obj.MapRoute("Default", "{Controller=Home}/{action=Index}/{Id?}");
        }
    }
}
