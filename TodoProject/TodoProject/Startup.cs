using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoProject.Commands.Account;
using TodoProject.Commands.Security;
using TodoProject.Data;
using TodoProject.General;
using TodoProject.Interfaces.Account;
using TodoProject.Interfaces.General;
using TodoProject.Interfaces.Security;
using TodoProject.Models;
using TodoProject.ResponseModels;
using TodoProject.Services.Account;
using TodoProject.Services.General;
using TodoProject.Services.Security;
using TodoProject.ViewModels.Account;
using TodoProject.ViewModels.Security;

namespace TodoProject
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
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddIdentity<ApplicationUser, IdentityRole>(IdentityConfig.Options)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(builder =>
            {
                builder.AddPolicy("DefaultPolicy", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, builder =>
                {
                    builder.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };

                    builder.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            UnauthorizedResponse response;

                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers.Add("Refresh_Token", "true");
                                response = new UnauthorizedResponse("Refresh Token");
                            }
                            else
                            {
                                response = new UnauthorizedResponse();
                            }

                            var bodyData = GetResponseBodyForTokenEvent(response);
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            context.Response.Body.WriteAsync(bodyData, 0, bodyData.Length);
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            var bodyData = GetResponseBodyForTokenEvent(new ForbiddenResponse());
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            context.Response.Body.WriteAsync(bodyData, 0, bodyData.Length);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            // Services
            services.AddTransient<IEmailGateway, SendGridEmailGateway>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IDeserializeToken, TokenService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IUserService, UserService>();

            // Commands
            services.AddTransient<ICommand<LoginModel>, LoginCommand>();
            services.AddTransient<ICommand<RefreshTokenModel>, RefreshTokenCommand>();
            services.AddTransient<ICommand<ChangePasswordModel>, ChangePasswordCommand>();
            services.AddTransient<ICommand<ResetPasswordRequestModel>, ResetPasswordRequestCommand>();
            services.AddTransient<ICommand<ResetPasswordModel>, ResetPasswordCommand>();
            services.AddTransient<ICommand<RegisterModel>, RegisterCommand>();
            services.AddTransient<ICommand<UpdateAccountModel>, UpdateAccountCommand>();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(new BadRequestResponse(context.ModelState));
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                SeedData.Seed(roleManager, userManager, context);
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                app.UseExceptionHandler("/error/500");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private byte[] GetResponseBodyForTokenEvent(ApiResponse response)
        {
            var json = JsonConvert.SerializeObject(response);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
