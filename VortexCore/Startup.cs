using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VortexCore.ManagersDB;
using VortexCore.ModelsDB;
using VortexCore.ModelsDB.MongoDB;
using VortexCore.Services.Authentication;
using VortexCore.Services.MongoDB;
using VortexCore.Services.Hubs;

namespace VortexCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddDbContext<VortexBDContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<ChatDatabaseSettings>(options =>
            {
                options.ChatCollectionName = "VortexChat";
                options.ConnectionString = "mongodb://localhost:27017";
                options.DatabaseName = "VortexDB";
            });
            services.AddSingleton<IChatDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ChatDatabaseSettings>>().Value);
            services.AddSingleton<ChatService>();

            services.AddSignalR();

            services.AddControllers();

            //services.AddAuthentication(FirebaseAuthenticationOptions.Scheme)
            //    .AddScheme<FirebaseAuthenticationOptions, FirebaseAuthenticationHandler>(FirebaseAuthenticationOptions.Scheme, options =>
            //    {
            //        /* configure options */
            //    });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = "https://securetoken.google.com/" + Configuration["FirebaseAppId"];
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = "https://securetoken.google.com/" + Configuration["FirebaseAppId"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["FirebaseAppId"],
                            ValidateLifetime = true,
                            NameClaimType = "name"
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];

                                // If the request is for our hub...
                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken) &&
                                    (path.StartsWithSegments("/chat")))
                                {
                                    // Read the token out of the query string
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            new Services.FirebaseControl();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
