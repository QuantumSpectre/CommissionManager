using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CommissionManager.API.Repositories;

namespace CommissionManager.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Allows controllers to be used
            services.AddControllers();

            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICommissionRepository, CommissionRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //sets the location of endpoints
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello from the CommissionManagerAPI!");
                });


                endpoints.MapControllers();
            });
        }


    }
}
