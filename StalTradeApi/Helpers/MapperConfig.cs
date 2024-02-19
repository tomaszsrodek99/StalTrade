﻿using AutoMapper;
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
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Deposit, DepositDto>().ReverseMap();
            CreateMap<StockStatus, StockStatusDto>().ReverseMap();
            CreateMap<Price, PriceDto>().ReverseMap();
        }
    }  
}
