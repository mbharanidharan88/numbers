using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Numbers.Database;
using Numbers.Models.DTO;
using Numbers.Service.Implementation;
using Numbers.Service.Interfaces;
using Numbers.Worker;
using System;
using System.Threading.Channels;


namespace Numbers.Web
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
                services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //services.AddHostedService<GeneratorManager>();
            //services.AddScoped<GeneratorManager>();

            services.AddSingleton<GeneratorManager>();
            services.AddHostedService(provider => provider.GetService<GeneratorManager>());

            services.AddSingleton<MultiplierManager>();
            services.AddHostedService(provider => provider.GetService<MultiplierManager>());

            services.AddSingleton<IProcessor, Processor>();
            
            services.AddSingleton(Channel.CreateUnbounded<GeneratorBatch>(new UnboundedChannelOptions() { SingleReader = true }));
            services.AddSingleton(provider => provider.GetRequiredService<Channel<GeneratorBatch>>().Reader);
            services.AddSingleton(provider => provider.GetRequiredService<Channel<GeneratorBatch>>().Writer);

            services.AddSingleton(Channel.CreateUnbounded<MultiplierBatch>(new UnboundedChannelOptions() { SingleReader = true }));
            services.AddSingleton(provider => provider.GetRequiredService<Channel<MultiplierBatch>>().Reader);
            services.AddSingleton(provider => provider.GetRequiredService<Channel<MultiplierBatch>>().Writer);

            services.AddHttpClient<IGeneratorService, GeneratorService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseUrl"]);
            });

            services.AddHttpClient<IMultiplierService, MultiplierService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseUrl"]);
            });

            //services.AddScoped<IBatchRetreivalService, BatchRetreivalService>();

            services.AddTransient<IBatchRepository, BatchRepository>();

            //EF In Memory Database
            services.AddDbContext<NumbersDbContext>(options => options.UseInMemoryDatabase(databaseName: "NumbersDB"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
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
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
