using AutoMapper;
using BeymenGroup.Shared.Dtos;
using Configuration.Business.Abstractions;
using Configuration.Common.ViewModels;
using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Business.Business
{
    public class ConfigurationBusiness : IConfigurationBusiness
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationStorage _configurationStorage;

        public ConfigurationBusiness(IMapper mapper, IConfigurationStorage configurationStorage)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configurationStorage = configurationStorage ?? throw new System.ArgumentNullException(nameof(configurationStorage));
        }

        public async Task<Response<List<ConfigViewModel>>> GetAllAsync()
        {
            try
            {
                var configList = await _configurationStorage.GetAllAsync();

                var result = _mapper.Map<List<ConfigViewModel>>(configList);

                return Response<List<ConfigViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                //merkezi hata yakalama ve loglama eklenebilir (middleware)
                return Response<List<ConfigViewModel>>.Fail(ex.Message);
            }
        }

        public async Task<Response<ConfigViewModel>> GetAsync(string id)
        {
            try
            {
                var config = await _configurationStorage.GetAsync(id);

                var result = _mapper.Map<ConfigViewModel>(config);

                return Response<ConfigViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<ConfigViewModel>.Fail(ex.Message);
            }
        }

        public async Task<Response<ConfigViewModel>> AddOrUpdateAsync(ConfigViewModel model)
        {
            try
            {
                var configModel = _mapper.Map<ConfigModel>(model);

                var result = await _configurationStorage.AddOrUpdateAsync(configModel);

                return Response<ConfigViewModel>.Success(_mapper.Map<ConfigViewModel>(result));
            }
            catch (Exception ex)
            {
                return Response<ConfigViewModel>.Fail(ex.Message);
            }
        }

        public async Task<Response<bool>> DeleteAsync(ConfigViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return Response<bool>.Fail("Id alanı boş olamaz!");
                }

                var configModel = _mapper.Map<ConfigModel>(model);

                var result = await _configurationStorage.DeleteAsync(configModel);

                return Response<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message);
            }
        }

        public Response<List<string>> GetValueTypeList()
        {
            return Response<List<string>>.Success(new List<string>
                {
                    typeof(string).FullName,
                    typeof(int).FullName,
                    typeof(bool).FullName,
                    typeof(double).FullName
                });
        }
    }
}