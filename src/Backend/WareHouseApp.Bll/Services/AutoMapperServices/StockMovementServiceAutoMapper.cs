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
        var movements = await context.StockMovements
                .Include(sm => sm.InventoryItem)           
                    .ThenInclude(i => i.WareHouse)          
                .Where(sm => sm.InventoryItem.ProductId == productId) 
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync(); 

        return mapper.Map<IEnumerable<StockMovementDto>>(movements);

    }
}
