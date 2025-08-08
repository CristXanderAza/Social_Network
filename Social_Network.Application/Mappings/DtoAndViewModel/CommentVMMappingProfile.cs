using AutoMapper;
using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Application.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Mappings.DtoAndViewModel
{
    public class CommentVMMappingProfile : Profile
    {
        public CommentVMMappingProfile()
        {
            CreateMap<CommentDTO, CommentReadVM>();

        }
    }
}
