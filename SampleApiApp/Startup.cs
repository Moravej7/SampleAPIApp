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
using WebFramework.Configuration;

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

            services.AddControllers();
            services.AddDBContext<AddSqlServer>(Configuration.GetConnectionString("SqlServer"));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(_siteSettings.CorsOrigins)
                        .AllowAnyHeader()
                        .AllowCredentials().AllowAnyMethod();
                    });
            }
            );
            services.AddSwaggerGen();
            services.AddMinimalMvc();
            services.AddCustomApiVersioning();
            services.AddCustomIdentity();
            services.AddJwtAuthentication(_siteSettings.JwtSettings);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                dbcontext.Database.Migrate();

                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                ApplicationDbInitializer.SeedUsers(userManager, roleManager);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
