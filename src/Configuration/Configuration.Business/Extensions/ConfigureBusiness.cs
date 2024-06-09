using Configuration.Business.Abstractions;
using Configuration.Business.Business;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Configuration.Business.Extensions
{
    public static class ConfigureBusiness
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IConfigurationBusiness, ConfigurationBusiness>();
        }
    }
}