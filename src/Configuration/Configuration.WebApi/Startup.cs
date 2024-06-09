using Configuration.Business.Extensions;
using Configuration.Common.Models;
using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.MongoDB.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Configuration.WebApi
{
    public class Startup
    {
        private const string policyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Configuration.WebApi", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000", "http://www.selenyum.com")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });

            services.Configure<AppSettings>(Configuration);
            services.AddDbContextRepositories();
            var dbContext = services.BuildServiceProvider().GetService<IConfigurationStorage>();//AddDbContextRepositories içinde olmasý gerek lakin .net5de BuildServiceProvider() çalýþmadý
            dbContext.Seed();
            services.AddBusiness();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Configuration.WebApi v1"));
            }

            app.UseCors(policyName);
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}