using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using Sneaker_DATN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Sneaker_DATN
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
            var mailsettings = _configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsettings);

            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();

            services.AddSession(option => { option.IdleTimeout = TimeSpan.FromMinutes(30); });

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IEncodeHelper, EncodeHelper>();
            services.AddTransient<IAdminSvc, AdminSvc>();
            services.AddTransient<IProductSvc, ProductSvc>();
            services.AddTransient<IUserSvc, UserSvc>();
            services.AddTransient<IUserMemSvc, UserMemSvc>();
            services.AddTransient<IUploadHelper, UploadHelper>();
            services.AddTransient<IOrderSvc, OrderSvc>();
            services.AddTransient<IOrderDetailSvc, OrderDetailSvc>();
            services.AddTransient<IDiscountSvc, DiscountSvc>();
            services.AddTransient<ISendMailService, SendGmailSvc>();

            //services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
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

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGet("/testmail", async context =>
                {
                    // Lấy dịch vụ sendmailservice
                    var sendmailservice = context.RequestServices.GetService<ISendMailService>();
                    MailContent content = new MailContent
                    {
                        To = "ngocdam2k@gmail.com",
                        Subject = "Kiểm tra thử",
                        Body = "<p><strong>Xin chào Dach</strong></p>"
                    };

                    await sendmailservice.SendMail(content);
                    await context.Response.WriteAsync("Send mail");
                });
            });
        }
    }
}
