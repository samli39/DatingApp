﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DatingApp.DAL;
using DatingApp.DAL.Auth;
using DatingApp.Helpher;
using DatingApp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp
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
            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder =>
                        builder.WithOrigins("http://localhost:4200"
                        ).AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
                }
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DatingDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DatingSQL")));
            services.AddScoped<ValuesDAL>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    //clarify what to validate
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        //check if the key is valid
                        ValidateIssuerSigningKey = true,
                        //generate the signingkey to compare
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token"])),
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();

                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        //sending back the status code as 500 server error
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        //get the exception
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            //adding message to response header
                            context.Response.AddApplicationError(error.Error.Message);
                            //writing error message into http response
                            await context.Response.WriteAsync(error.Error.Message);
                        }

                    });
                });
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
