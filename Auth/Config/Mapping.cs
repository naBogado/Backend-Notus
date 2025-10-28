using Notus.Models.User;
using Notus.Models.User.Dto;
using AutoMapper;

namespace Notus.Config
{
    public class Mapping : Profile
    {
        public Mapping() {
            // Defaults
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);

            // Auth
            CreateMap<RegisterDTO, User>();

            CreateMap<UpdateUserDTO, User>().ForAllMembers(opts =>
            {
                opts.Condition((_, _, srcMember) => srcMember != null);
            });

            CreateMap<User, UserWithoutPassDTO>().ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(e => e.Roles.Select(x => x.Name).ToList())
            );


        }
    }
}
