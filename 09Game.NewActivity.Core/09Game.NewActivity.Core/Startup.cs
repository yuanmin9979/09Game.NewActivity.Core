using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using _09Game.NewActivity.Core.Authentication;
using _09Game.NewActivity.Core.Authentication.DbIWork;
using _09Game.NewActivity.Core.Authentication.DbWork;
using _09Game.NewActivity.Core.Enity;
using _09Game.NewActivity.Core.Enity.DBConnect;
using _09Game.NewActivity.Core.Util;
using _09Game.NewActivity.Core.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rabbit;

namespace _09Game.NewActivity.Core
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
            services.AddMemoryCache();
            services.AddControllers();

            services.AddDbContext<UserDbContext>(opt => opt.UseMySql(Configuration.GetConnectionString("user.db")));
            services.AddDbContext<PifuDbContext>(opt => opt.UseMySql(Configuration.GetConnectionString("pifu.db")));

            services.AddScoped<UserDbWork>(_ => new UserDbWork(Configuration.GetConnectionString("user.db")));
            services.AddScoped<PifuDbWork>(_ => new PifuDbWork(Configuration.GetConnectionString("pifu.db")));

            services.AddScoped<Token>();
            services.AddScoped<UserDb>();
            services.AddScoped<PifuDb>();

            services.AddScoped<UserWork>();
            services.AddScoped<PifuWork>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true) // =AllowAnyOrigin()
                .AllowCredentials();
            }));

            //services.AddHostedService<ChapterLister>();
            //services.AddSingleton<RabbitMQClient, RabbitMQClient>();

            services.AddMvc();

            services.AddControllersWithViews();

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            Task.Run(() =>
            {
                while (true)
                {
                    VisitLog.saveLog();
                    Thread.Sleep(60 * 1000 * 3);
                }
            });

            app.UsePerformanceLog();

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

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region MyRegion
            //this.Configuration.ConsulRegist();
            #endregion
        }
    }
}
