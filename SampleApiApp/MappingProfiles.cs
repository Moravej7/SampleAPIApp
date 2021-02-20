using AutoMapper;
using Entities.User;
using Factor.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApiApp
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<GetUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<EditUserDto, User>();
            CreateMap<User, EditUserDto>();

        }
    }
}
