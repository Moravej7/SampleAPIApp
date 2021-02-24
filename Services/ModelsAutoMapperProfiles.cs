using AutoMapper;
using Entities;
using Services.Models;

namespace Services
{
    public class ModelsAutoMapperProfile : Profile
    {
        public ModelsAutoMapperProfile()
        {
            CreateMap<Company, CreateCompanyModel>();
            CreateMap<CreateCompanyModel, Company>();
            CreateMap<Company, GetCompanyModel>();
            CreateMap<GetCompanyModel, Company>();
        }
    }
}
