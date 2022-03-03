using AutoMapper;
using MovieChatacterAPI.Models;
using MovieChatacterAPI.Models.Domain;
using System.Linq;

namespace MovieChatacterAPI.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieCreateDTO>()
                .ForMember(mdto => mdto.Characters, opt =>
                opt.MapFrom(m => m.Characters.Select(c => c.Id).ToArray()))
                .ReverseMap();
            CreateMap<Movie, MovieEditDTO>()
                .ForMember(mdto => mdto.Characters, opt =>
                opt.MapFrom(m => m.Characters.Select(c => c.Id).ToArray()))
                .ReverseMap();
            CreateMap<Movie, MovieReadDTO>()
                .ForMember(mdto => mdto.Characters, opt =>
                opt.MapFrom(m => m.Characters.Select(c => c.Id).ToArray()))
                .ReverseMap();
        }
    }
}
