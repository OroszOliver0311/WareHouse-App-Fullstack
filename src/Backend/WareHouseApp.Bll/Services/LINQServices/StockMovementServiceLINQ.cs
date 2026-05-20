using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.LINQServices;

public class StockMovementServiceLINQ(AppDbContext context) : IStockMovementService
{
    public async Task<IEnumerable<StockMovementDto>> GetProductHistoryAsync(int productId)
    {
        return await context.StockMovements
            .Include(m => m.InventoryItem)
                .ThenInclude(i => i.WareHouse)
            .Where(m=> m.InventoryItem.ProductId == productId)
            .OrderByDescending(m=> m.MovementDate)
            .Select(m => new StockMovementDto
            {
                Id = m.Id,
                WareHouseLocation = m.InventoryItem.WareHouse.Location,
                IsIncoming = m.IsIncoming,
                Quantity = m.Quantity,
                Date = m.MovementDate 
            })
            .ToListAsync();
    }
}
