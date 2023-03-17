using AutoMapper;
using Curso.Core.Configuration;
using Curso.Core.Data;
using Curso.Core.Service.FileDataService;
using Curso.Core.Service.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Curso.Core.Api
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
            services.AddHttpContextAccessor();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Curso.Core.Api", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            //-- AutoMapper -->
            services.AddAutoMapper(typeof(Service.ClassMapper));


            var assembly = AppDomain.CurrentDomain.Load("Curso.Core.Service");
            services.AddMediatR(assembly);

            // configure jwt authentication
            //var key = System.Text.Encoding.UTF8.GetBytes(appSettings.Jwt.Key);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigureSettings.GetJwtConfig().Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // database..
            var databaseService = ConfigureSettings.GetService(ConfigureSettings.SecaVoceService);
            switch (databaseService.Repository.Provider)
            {
                case "SqlServer":
                    services.AddDbContext<CoreDbContext>(options =>
                        options.UseSqlServer(databaseService.Repository.ConnectionString));
                    break;
                case "MySql":
                    services.AddDbContext<CoreDbContext>(options =>
                        options.UseMySql(databaseService.Repository.ConnectionString,
                            new MySqlServerVersion(new Version(8, 0, 21)), // Replace with your server version and type.
                            mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)));
                    break;
                default:
                    throw new Exception("DatabaseType not configured");
            }

            // filedata
            var appSettings = ConfigureSettings.GetSettings();
            switch (appSettings.FileDataSettings.FileDataStorage)
            {
                case Core.Configuration.EFileDataStorage.AWS:
                    services.AddScoped<IStorageService, AWSStorage>();
                    break;
                case Core.Configuration.EFileDataStorage.Azure:
                    services.AddScoped<IStorageService, AzureStorage>();
                    break;
                case Core.Configuration.EFileDataStorage.CGP:
                    services.AddScoped<IStorageService, GoogleCloudStorage>();
                    break;
                default:
                    services.AddScoped<IStorageService, LocalStorage>();
                    break;
            }

            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CoreDbContext dbContext)
        {
            // migrate any database changes on startup (includes initial db creation)
            //dbContext.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Curso.Core.Api");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
