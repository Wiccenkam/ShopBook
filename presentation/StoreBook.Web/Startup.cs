using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopBook;
using ShopBook.Contractors;
using ShopBook.Messages;
using store.Contractors;
using Store.Web.Contractors;
using Store.Contractors.YandexKassa;
using Store.Memory;
using System;
using Store.Web.Application;

namespace StoreBook.Web
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
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton<IBookRepository, BookRepositiry>();
            services.AddSingleton<BookService>();
            services.AddSingleton<OrderService>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<INotificationService, DebugNotificationService>();
            services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
            services.AddSingleton<IPaymentService, CashPaymentService>();
            services.AddSingleton<IPaymentService, YandexKassaPaymentService>();
            services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();
           
            
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
         
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                /*endpoints.MapAreaControllerRoute(
                    name: "yandex.kassa",
                    areaName:"YandexKassa",
                    pattern: "YandexKassa/{controller=Home}/{action=Index}/{id?}");*/
            });
        }
    }
}
