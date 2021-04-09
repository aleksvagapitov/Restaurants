using System.Text;
using Application.Interfaces;
using API.Middleware;
using Domain.Entities;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using AutoMapper;
using Infrastructure.Photos;
using API.SignalR;
using System.Threading.Tasks;
using Application.Users.Profiles;
using System;
using Application.User;
using Infrastructure.Emails;
using Infrastructure.Locations;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
           {
               options.UseNpgsql(Configuration.GetConnectionString("DatabaseConnection"),
                                            x=> x.UseNetTopologySuite());
           });

           services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
           services.Configure<MinioSettings>(Configuration.GetSection("Minio"));
           services.Configure<LocationApiParameters>(Configuration.GetSection("LocationApiParameters"));

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
           {
               //options.UseLazyLoadingProxies();
               options.UseNpgsql(Configuration.GetConnectionString("DatabaseConnection"),
                                                x=> x.UseNetTopologySuite());
           });
           
           services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
           services.Configure<MinioSettings>(Configuration.GetSection("Minio"));
           services.Configure<LocationApiParameters>(Configuration.GetSection("LocationApiParameters"));

            ConfigureServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
           {
               opt.AddPolicy("CorsPolicy", policy =>
               {
                   //policy.AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("WWW-Authenticate").WithOrigins("http://localhost:3000").AllowCredentials();
                   policy.AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("WWW-Authenticate").WithOrigins("*");//.AllowCredentials();
               });
           });

            services.AddMediatR(typeof(Login.Handler).Assembly);
            services.AddAutoMapper(typeof(Login.Handler));
            services.AddSignalR();
            services.AddControllers(opt =>
               {
                   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                   opt.Filters.Add(new AuthorizeFilter(policy));
               })
                .AddFluentValidation(cfg =>
               {
                   cfg.RegisterValidatorsFromAssemblyContaining<Login>();
               });

            // Adding IdentityCore to the Project
            var userBuilder = services.AddIdentityCore<AppUser>();
            var identityBuilder = new IdentityBuilder(userBuilder.UserType, userBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();
            identityBuilder.AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsRestaurantOwner", policy =>
                {
                    policy.Requirements.Add(new IsRestaurantOwner());
                });
            });

            services.AddTransient<IAuthorizationHandler, IsRestaurantOwnerHandler>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
               {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = key,
                       ValidateAudience = false,
                       ValidateIssuer = false,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero
                   };
                   opt.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                           {
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IProfileReader, ProfileReader>();
            services.AddScoped<ILocationConverter, LocationConverter>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage ();
            }

            // app.UseHttpsRedirection ();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
               endpoints.MapHub<ChatHub>("/chat");
           });
        }
    }
}