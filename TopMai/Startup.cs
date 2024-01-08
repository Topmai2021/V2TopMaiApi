using Infraestructure.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TopMai.Handlers;

namespace TopMai
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger 1/2

            SwaggerHandler.SwaggerConfig(services);

            #endregion Swagger 1/2

            #region Context SQL Server
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            });
            #endregion
            
            #region Inyeccion de dependencia 
            DependencyInyectionHandler.DependencyInyectionConfig(services);
            #endregion

            #region Jwt Token Configuration 1/2
            IConfigurationSection tokenAppSetting = Configuration.GetSection("Tokens");
            JwtConfigurationHandler.ConfigureJwtAuthentication(services, tokenAppSetting);

            #endregion Jwt Configuration

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Jwt Token Configuration 2/2
            JwtConfigurationHandler.ConfigureUseAuthentication(app);
            #endregion


            #region Swagger 2/2

            SwaggerHandler.UseSwagger(app);

            #endregion Swagger 2/2

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
        

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
