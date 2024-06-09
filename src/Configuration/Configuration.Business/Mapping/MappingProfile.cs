using AutoMapper;
using Configuration.Common.ViewModels;
using Configuration.Data.Models.Entities;

namespace Configuration.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ConfigViewModel, ConfigModel>().ReverseMap();
        }
    }
}
