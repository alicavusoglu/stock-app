using StokApp.Models;
using StokApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokApp.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<StockTransactionEditVM, StockTransaction>()
                .ForMember(dest => dest.Input, opts => opts.MapFrom(src => src.TransactionType == StockTransactionType.Input ? src.Amount : 0))
                .ForMember(dest => dest.Output, opts => opts.MapFrom(src => src.TransactionType == StockTransactionType.Output ? src.Amount : 0));

            AutoMapper.Mapper.CreateMap<StockTransaction, StockTransactionEditVM>()
        .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.Input == 0 ? src.Output : src.Input))
        .ForMember(dest => dest.TransactionType, opts => opts.MapFrom(src => src.Input == 0 ? StockTransactionType.Output : StockTransactionType.Input));

            //AutoMapper.Mapper.CreateMap<Product, ProductEditVMv>();
            //AutoMapper.Mapper.CreateMap<ProductEditVMv, Product>();
        }
    }

}
