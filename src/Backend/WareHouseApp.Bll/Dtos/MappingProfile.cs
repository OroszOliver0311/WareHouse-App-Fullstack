using AutoMapper;
using WareHouse_App.Entities;

namespace WareHouseApp.Bll.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        //ProductServiceMapping
        CreateMap<Product, Dtos.ProductDashboardDto>()
            .ForMember(dest => dest.TotalQuantity ,
            opt => opt.MapFrom(src => src.InventoryItems.Sum(i => i.Quantity)));
        CreateMap<InventoryItem, Dtos.ProductDetailWareHouseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WareHouseId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WareHouse.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.WareHouse.Location));
        
        CreateMap<Product, Dtos.ProductDetailDto>()
            .ForMember(dest => dest.Stocks, opt => opt.MapFrom(src => src.InventoryItems));
        CreateMap<CreateProductDto, Product>();
        //WareHouseServiceMapping
        CreateMap<Warehouse, Dtos.WareHouseDto>();
        CreateMap<CreateWareHouseDto, Warehouse>();

    }
}
