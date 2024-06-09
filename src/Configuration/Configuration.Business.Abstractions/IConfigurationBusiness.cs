using BeymenGroup.Shared.Dtos;
using Configuration.Common.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Business.Abstractions
{
    public interface IConfigurationBusiness
    {
        Task<Response<List<ConfigViewModel>>> GetAllAsync();

        Task<Response<ConfigViewModel>> GetAsync(string id);

        Task<Response<ConfigViewModel>> AddOrUpdateAsync(ConfigViewModel model);

        Task<Response<bool>> DeleteAsync(ConfigViewModel model);

        Response<List<string>> GetValueTypeList();
    }
}