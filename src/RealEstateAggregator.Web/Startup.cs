using System.Net.Http;
using Exceptionless;
using RealEstateAggregator.Web.Hubs;
using RealEstateAggregator.Web.Repositories;
using RealEstateAggregator.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RealEstateAggregator.Web
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
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddSingleton<HttpClient>();
            services.AddTransient<IScrappingService, MorizonScrappingService>();
            services.AddTransient<IScrappingService, OtoDomScrappingService>();
            services.AddTransient<IScrappingService, OlxScrappingService>();
            //services.AddTransient<IScrappingService, DomImportaScrappingService>();
            services.AddTransient<IScrappingService, GumtreeScrappingService>();
            services.AddTransient<IScrappingService, GratkaScrappingService>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddSingleton<ISingletonDataService, SingletonDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseExceptionless("3yzjMfHLDzsOuZSMo5rRMowX5N9NVpyZncMFhQeT");//

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DynamicResultsHub>("/dynamicResultsHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Search}/{action=Index}/{id?}");
            });
        }
    }
}
