using AutoMapper;
using MovieChatacterAPI.Models;
using MovieChatacterAPI.Models.Domain;
using System.Linq;

namespace MovieChatacterAPI.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterCreateDTO>()
                .ForMember(cdto => cdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
            CreateMap<Character, CharacterEditDTO>()
                .ForMember(cdto => cdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
            CreateMap<Character, CharacterReadDTO>()
                .ForMember(cdto => cdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
        }
    }
}
