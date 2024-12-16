using AutoMapper;
using Server.DTOs;
using Server.Models;

namespace Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();

            // Role Mappings
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, RoleCreateDTO>().ReverseMap();

            // Arena Mappings
            CreateMap<Arena, ArenaDTO>().ReverseMap();
            CreateMap<Arena, ArenaCreateDTO>().ReverseMap();

            // Game Mappings
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<Game, GameCreateDTO>().ReverseMap();
        }
    }
}
