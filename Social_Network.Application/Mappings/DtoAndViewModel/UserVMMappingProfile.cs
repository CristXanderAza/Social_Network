using AutoMapper;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Mappings.DtoAndViewModel
{
    public class UserVMMappingProfile : Profile
    {
        public UserVMMappingProfile()
        {
            CreateMap<LoginVM, LoginRequestDTO>();
            CreateMap<RegisterRequestVM, RegisterRequestDTO>();
            CreateMap<UserHeaderDTO,UserHeaderVM>();
        }
    }
}
