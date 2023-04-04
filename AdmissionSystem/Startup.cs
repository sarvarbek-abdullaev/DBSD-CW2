using AdmissionSystem.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace AdmissionSystem
{
    public class Startup
    {
        private const string DataDirectory = "|DataDirectory|";
        private string _appPath;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _appPath = Directory.GetCurrentDirectory();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<ITeacherRepository>(
                x => new TeacherRepository(Configuration.GetConnectionString("CW")
                    .Replace(DataDirectory, _appPath))
            );
            
            services.AddScoped(
                x => new StudentRepository(Configuration.GetConnectionString("CW")
                    .Replace(DataDirectory, _appPath))
            );
            
            services.AddScoped<IClassRepository>(
                x => new ClassRepository(Configuration.GetConnectionString("CW")
                    .Replace(DataDirectory, _appPath))
            );

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _appPath = Path.Combine(env.ContentRootPath, "AppData");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
