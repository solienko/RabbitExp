using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace PublisherWeb
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
            services.AddMvc();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrWhiteSpace(Configuration["EventBusPassword"]))
                {
                    factory.UserName = Configuration["EventBusPassword"];
                }

                var retryCount = 5;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);

            });

            RegisterEventBus(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, MyEventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = Configuration["SubscriptionClientName"];

                var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<MyEventBusRabbitMQ>>();
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var subsManager = sp.GetRequiredService<IEventBusSubscriptionManager>();

                return new MyEventBusRabbitMQ(connection, logger, scopeFactory, subsManager, subscriptionClientName);
            });

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
