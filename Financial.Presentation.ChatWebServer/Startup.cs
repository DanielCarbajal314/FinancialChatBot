﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financial.Infrastructure.EFDataPersistance;
using Financial.Infrastructure.MessageQueu;
using Financial.Infrastructure.MessageQueu.Configuration;
using Financial.Presentation.ChatWebServer.BackgroundTask;
using Financial.Presentation.ChatWebServer.Filters;
using Financial.Presentation.ChatWebServer.Hubs;
using Financial.Services.EFImplementation;
using Financial.Services.Interfaces.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financial.Presentation.ChatWebServer
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ChatContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ChatDb"));
                
            });
            services.AddTransient<IChatHandler, ChatHandler>();
            services.AddTransient<IRabbitStockQuery, RabbitStockQuery>();
            services.AddTransient<IRabbitStockResponse, RabbitStockResponse>();
            services.Configure<RabbitMqConnectionSettings>(Configuration.GetSection("RabbitMqConnectionSettings"));
            services.AddScoped<ExceptionFilter>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
            services.AddHostedService<StockResponseBackgroundMessageBroker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
