using AutoMapper;
using StalTradeApi.Dtos;

namespace StalTradeApi.Helpers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Models.User, UserDto>().ReverseMap();
        }
    }
}
