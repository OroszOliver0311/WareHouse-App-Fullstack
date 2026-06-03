using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Dtos.Encoding;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.LINQServices;

public class InventoryServiceLINQ(AppDbContext context, IDateTimeProvider dateTimeProvider, IIdEncoder idEncoder) : IInventoryService
{
    public async Task UpsertInventoryAsync(InventoryItemDto upsertItem)
    {
        int productId = idEncoder.Decode(upsertItem.ProductId);
        int wareHouseId = idEncoder.Decode(upsertItem.WareHouseId);

        var inventoryItem = await context.InventoryItems
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WareHouseId == wareHouseId);
        int currentQty = 0;
        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                ProductId = productId,
                WareHouseId = wareHouseId,
                Quantity = 0 
            };
            context.InventoryItems.Add(inventoryItem);
        }
        else
        {
            currentQty = inventoryItem.Quantity;
        }
        int difference = upsertItem.Quantity - currentQty;
        if (difference == 0) return;
        inventoryItem.Quantity = upsertItem.Quantity;

        //Sideffect or Yagni //TODO
        var movement = new StockMovement
        {
            InventoryItem = inventoryItem,
            IsIncoming = difference > 0,
            Quantity = Math.Abs(difference),
            MovementDate = dateTimeProvider.UtcNow
        };

        context.StockMovements.Add(movement);
        await context.SaveChangesAsync();
    }
}
