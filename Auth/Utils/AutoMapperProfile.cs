using AutoMapper;
using Notus.Models.Class.Dto;
using Notus.Models.Class;

namespace Notus.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Class, ClassWithNames>().ReverseMap();
        }
    }
}
