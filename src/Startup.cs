using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShortLinkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkApiCore
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ShortenDBContext>(
                    options=>options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
                );
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapFallback(HandleFallback);
            });
        }

        private Task HandleFallback(HttpContext context)
        {
            if (context.Request.Path == "/")
            {
                return context.Response.SendFileAsync("wwwroot/index.htm");
            }

            // Default to home page if no matching url.
            var redirect = "/";

            var db = context.RequestServices.GetService<ShortenDBContext>();
         

            var path = context.Request.Path.ToUriComponent().Trim('/');
            var id = ShortLink.GetId(path);
            //var entry = db.ShortLinks.Find(id);

            //if (entry is not null)
            //{
            //    redirect = entry.Url;
            //}

            //context.Response.Redirect(redirect);


            redirect = $"{context.Request.Scheme}://{context.Request.Host}/redirect?id={id}";
            context.Response.Redirect(redirect);

            return Task.CompletedTask;


        }
    }
}
