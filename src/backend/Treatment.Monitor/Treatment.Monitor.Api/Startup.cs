using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Treatment.Monitor.BusinessLogic.Configuration;
using Treatment.Monitor.BusinessLogic.Services;
using Treatment.Monitor.DataLayer;
using Treatment.Monitor.DataLayer.Repositories;
using Treatment.Monitor.DataLayer.Sys;
using Treatment.Monitor.Extensions;
using Treatment.Monitor.Filters;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            var connectionString = Configuration.GetConnectionString(Consts.DatabaseName);
            var connectionStringVar = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(connectionStringVar))
            {
                connectionString = connectionStringVar;
            }

            var context = new TreatmentMonitorContext(connectionString);
            services.AddSingleton(context);

            IConfigurationSection securityConfigSection = Configuration.GetSection(nameof(SecuritySettings));
            var securitySettings = securityConfigSection.Get<SecuritySettings>();
            services.AddSingleton<ISecuritySettings>(securitySettings);

            services.AddControllers(options => { options.Filters.Add<ServiceActionFilter>(); });

            services.AddSingleton<IGenericRepository<TreatmentModel>, GenericRepository<TreatmentModel>>();
            services.AddScoped<ITreatmentService, TreatmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var securitySettings = app.ApplicationServices.GetService<ISecuritySettings>();

            app.UseCors(options =>
                options
                    .WithOrigins(securitySettings?.AllowedHost)
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.ConfigureExceptionHandler();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}