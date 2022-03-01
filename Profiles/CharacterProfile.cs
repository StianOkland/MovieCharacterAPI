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
            CreateMap<Character, CharacterDTO>()
                .ForMember(cdto => cdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
        }
    }
}
