using AutoMapper;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos.Encoding;

namespace WareHouseApp.Bll.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        //ProductServiceMapping
        CreateMap<Product, Dtos.ProductDashboardDto>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing<IntToSqidConverter, int>(src => src.Id))
            .ForMember(dest => dest.TotalQuantity ,opt => opt.MapFrom(src => src.InventoryItems.Sum(i => i.Quantity)));
       
        CreateMap<InventoryItem, Dtos.ProductDetailWareHouseDto>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing<IntToSqidConverter, int>(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WareHouse.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.WareHouse.Location));
        
        CreateMap<Product, Dtos.ProductDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing<IntToSqidConverter, int>(src => src.Id))
            .ForMember(dest => dest.Stocks, opt => opt.MapFrom(src => src.InventoryItems));
        CreateMap<CreateProductDto, Product>();
        //WareHouseServiceMapping
        CreateMap<Warehouse, Dtos.WareHouseDto>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing<IntToSqidConverter, int>(src => src.Id));
        CreateMap<CreateWareHouseDto, Warehouse>();
        //StockMovementServiceMapping
        CreateMap<StockMovement, Dtos.StockMovementDto>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing<IntToSqidConverter, int>(src => src.Id))
            .ForMember(dest => dest.WareHouseLocation,opt => opt.MapFrom(src => src.InventoryItem.WareHouse.Location))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.MovementDate));
    }
}
