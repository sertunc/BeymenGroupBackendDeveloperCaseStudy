using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.Data.MongoDB.Extensions
{
    public static class ConfigureDbContext
    {
        public static void AddDbContextRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationStorage, ConfigurationStorage>();
        }
    }
}