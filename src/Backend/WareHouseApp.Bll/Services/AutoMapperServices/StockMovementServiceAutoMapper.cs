using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.AutoMapperServices;

public class StockMovementServiceAutoMapper(AppDbContext context, IMapper mapper) : IStockMovementService
{
    public async Task<IEnumerable<StockMovementDto>> GetProductHistoryAsync(int productId)
    {
        return await context.StockMovements
            .Where(sm => sm.InventoryItem.ProductId == productId)
            .OrderByDescending(sm => sm.MovementDate)
            .ProjectTo<StockMovementDto>(mapper.ConfigurationProvider)
            .ToListAsync();

    }
}
