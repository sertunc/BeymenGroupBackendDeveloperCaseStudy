using Configuration.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Data.Abstractions.Interfaces
{
    public interface IConfigurationStorage
    {
        void Seed();

        Task<List<ConfigModel>> GetAllAsync();

        Task<ConfigModel> GetAsync(string id);

        Task<List<ConfigModel>> GetAllByApplicationNameAsync(string applicationName);

        Task<ConfigModel> AddOrUpdateAsync(ConfigModel model);

        Task<bool> DeleteAsync(ConfigModel model);
    }
}