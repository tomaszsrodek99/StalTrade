using AutoMapper;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

namespace StalTradeAPI.Helpers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(src => src.Prices))
                .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.StockStatus));

            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Deposit, DepositDto>().ReverseMap();
            CreateMap<StockStatus, StockStatusDto>().ReverseMap();
            CreateMap<Price, PriceDto>().ReverseMap()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceProduct, InvoiceProductDto>()
                .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.Invoice))
                .ReverseMap();
        }
    }  
}
