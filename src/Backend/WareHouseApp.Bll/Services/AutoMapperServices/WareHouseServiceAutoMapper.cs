using AutoMapper;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.AutoMapperServices;

public class WareHouseServiceAutoMapper(AppDbContext context, IMapper mapper) : IWareHouseService
{


}
