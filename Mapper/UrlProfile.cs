using AutoMapper;
using Shortener.DTO;
using Shortener.Model;

namespace Shortener.Mapper
{
    public class UrlProfile : Profile
    {
        public UrlProfile()
        {
            CreateMap<UrlData, UrlDataDto>().ReverseMap();
            CreateMap<UrlData, ShortUrlDto>().ReverseMap();
        }
    }
}