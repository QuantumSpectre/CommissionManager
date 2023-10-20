using CommissionManager.Data;
using CommissionManager.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommissionManager.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICommissionRepository, CommissionRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlite());

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        }
    }


}
