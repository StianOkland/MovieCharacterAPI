using AutoMapper;
using MovieChatacterAPI.Models;
using MovieChatacterAPI.Models.Domain;
using System.Linq;

namespace MovieChatacterAPI.Profiles
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseCreateDTO>()
                .ForMember(fdto => fdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
            CreateMap<Franchise, FranchiseEditDTO>()
                .ForMember(fdto => fdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
            CreateMap<Franchise, FranchiseReadDTO>()
                .ForMember(fdto => fdto.Movies, opt =>
                opt.MapFrom(m => m.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();
        }
    }
}
