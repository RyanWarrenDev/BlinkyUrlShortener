using Blinky.Core.Repositories;
using Blinky.Core.Services;
using Blinky.Data;
using Blinky.Data.Repositories;
using Blinky.Extensions;
using Blinky.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blinky
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddMemoryCache();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpContextAccessor();

            services.AddTransient<IShortUrlRepository, ShortUrlRepository>();
            services.AddTransient<IShortTokenGenerator, ShortTokenGenerator>();
            services.AddTransient<IShortUrlService, ShortUrlService>();

            services.AddBlinkyContext(Configuration.GetConnectionString("blinky"));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                //endpoints.MapFallback(HandleRedirect);
            });
        }

        private static Task HandleRedirect(HttpContext context)
        {
            var path = context.Request.Path.ToUriComponent().Trim('/');
    
            var shortRepository = context.RequestServices.GetService<IShortUrlRepository>();
            var shortLink = shortRepository.GetShortUrlByToken(path);

            if (shortLink != null)
                context.Response.Redirect(shortLink.OriginalUrl);

            return Task.CompletedTask;
        }
    }
}
