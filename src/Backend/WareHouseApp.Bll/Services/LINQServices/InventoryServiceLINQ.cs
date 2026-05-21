using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.LINQServices;

public class InventoryServiceLINQ(AppDbContext context, IDateTimeProvider dateTimeProvider) : IInventoryService
{
    public async Task UpsertInventoryAsync(InventoryItemDto upsertItem)
    {
       var inventoryItem = await context.InventoryItems
            .FirstOrDefaultAsync(i => i.ProductId == upsertItem.ProductId && i.WareHouseId == upsertItem.WareHouseId);
        int currentQty = 0;
        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                ProductId = upsertItem.ProductId,
                WareHouseId = upsertItem.WareHouseId,
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
