using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VanillaPong.GameCode;
using VanillaPong.Hubs;
using VanillaPong.Models;

namespace VanillaPong
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add the razor pages service
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            // add signalr for realtime messaging
            services.AddSignalR();

            // add singleton for hub state
            services.AddSingleton<HubState>();
            // add singleton for lobby state sender
            services.AddSingleton<LobbyStateSender>();
            // add transient hub service
            services.AddTransient<HubService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // sets up the wwwroot folder to serve static files from the relative root of the website
            // e.g. wwwroot/js/app.js = https://localhost:1234/js/app.js
            app.UseStaticFiles();

            // Include the routing middleware - allows you to specify routes or in our case to map razor pages routes
            app.UseRouting();

            // Set up the razor pages routing so that the name of the pages corresponds to the URL
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<GameHub>("/gameHub");
            });
        }
    }
}
