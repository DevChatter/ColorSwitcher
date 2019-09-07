using System.Reflection;
using ColorSwitcher.Core;
using ColorSwitcher.Core.TwitchAuth;
using ColorSwitcher.Web.Hubs;
using ColorSwitcher.Web.Hubs.Emitters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;

namespace ColorSwitcher.Web
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
            services.Configure<TwitchSettings>(Configuration.GetSection("TwitchSettings"));

            services.AddSingleton<ChatBot>();
            services.AddSingleton<ITwitchClient, TwitchClient>();

            services.AddSingleton<TwitchAuthentication>();

            services.AddTransient<IColorEmitter, ColorEmitter>();

            services.AddMediatR(Assembly.GetAssembly(typeof(AccessTokenReceived)));

            services.AddMvc();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseDefaultFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToFile("", "index.html");
                endpoints.MapHub<ColorHub>("/ColorHub");
                endpoints.MapControllers();
            });
        }
    }
}
