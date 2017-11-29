using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using Zarasa.Editorial.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Helper;

namespace Zarasa.Editorial.Api
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
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            services.AddCors();
            services.AddMvc();
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IEmailSender, AuthMessageSender>();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTSettings:SecretKey"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = false,
                // ValidIssuer = Configuration["JWTSettings:Issuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = false,
                // ValidAudience = Configuration["JWTSettings:Audience"]
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                

            }).AddJwtBearer(options =>
            {
                // options.ClaimsIssuer = Configuration["JWTSettings:Issuer"];
                // options.Audience = Configuration["JWTSettings:Audience"];
                options.TokenValidationParameters = tokenValidationParameters;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(
                // options => options.WithHeaders("Access-Control-Allow-Origin").AllowAnyOrigin() WithOrigins("http://localhost:4488").AllowAnyMethod()
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                // options => options.WithHeaders("Access-Control-Allow-Origin:*").AllowAnyOrigin().AllowAnyMethod()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            // app.UseIdentity();

                 

            app.UseMvc();
        }
    }
}
