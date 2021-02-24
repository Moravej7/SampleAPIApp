using AutoMapper;
using Entities;
using Entities.User;
using Factor.DataTransferObjects;
using SampleApiApp.DataTransferObjects;
using Services.Models;

namespace SampleApiApp
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<GetUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<EditUserDto, User>();
            CreateMap<User, EditUserDto>();

            CreateMap<CreateCompanyDto, CreateCompanyModel>();
            CreateMap<GetCompanyModel, GetCompanyDto>();
        }
    }
}
