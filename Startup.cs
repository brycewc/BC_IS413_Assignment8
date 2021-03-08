using BC_IS413_Assignment8.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BC_IS413_Assignment8
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AmazonBooksDBContext>(options =>
               {
                   //Connection string. Defined in appsettings.json
                   options.UseSqlite(Configuration["ConnectionStrings:AmazonBooksConnection"]);
               });

            services.AddScoped<IAmazonBooksRepository, EFAmazonBooksRepository>();

            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession(); //By default, a session saves as long as it's running. With cookies and such, we can continue to hold past that. 
            //Can hold integers, strings, and bytes.
            //Convert to JSON if anything else is needed, like a database by converting to key-value pairs

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("categorypage",
                    "{category}/{pageNum:int}",
                    new { Controller = "home", action = "Index" });

                endpoints.MapControllerRoute("page",
                    "{pageNum:int}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapControllerRoute("category",
                    "{category}",
                    new { Controller = "Home", action = "Index", pageNum = 1 });

                endpoints.MapControllerRoute("pagination",
                    "Books/{pageNum}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });

            SeedData.EnsurePopulated(app); //For seeding data from SeedData.cs
        }
    }
}
