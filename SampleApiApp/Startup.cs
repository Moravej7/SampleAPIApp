using Common;
using Data;
using Entities.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using WebFramework.Configuration;
using AutoMapper;
using Serilog;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System;

namespace SampleApiApp
{
    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDBContext<AddSqlServer>(Configuration.GetConnectionString("SqlServer"));

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins(_siteSettings.CorsOrigins)
            //            .AllowAnyHeader()
            //            .AllowCredentials().AllowAnyMethod();
            //        });
            //}
            //);
            AddAutomepper(services);
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "http",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                          },
                          Scheme = "oauth2",
                          Name = "Authorization",
                          In = ParameterLocation.Header,
                          Flows = new OpenApiOAuthFlows(),
                          Type = SecuritySchemeType.ApiKey,
                        },
                        new List<string>()
                      }
                    });
            });
            services.AddCustomIdentity(_siteSettings.IdentitySettings);
            services.AddJwtAuthentication(_siteSettings.JwtSettings);
            services.AddMinimalMvc();
            //services.AddCustomApiVersioning();
            services.AddApplicationServices();
            services.AddConfig<SiteSettings>(Configuration, "SiteSettings");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");

            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                dbcontext.Database.Migrate();

                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                ApplicationDbInitializer.SeedUsers(userManager, roleManager);
            }

            //app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



        private void AddAutomepper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DtoMappingProfile());
                mc.AddProfile(new ModelsAutoMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
