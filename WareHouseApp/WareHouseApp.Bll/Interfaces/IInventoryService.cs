using WareHouseApp.Bll.Dtos;

namespace WareHouseApp.Bll.Interfaces;

public interface IInventoryService
{
    Task UpsertInventoryAsync(InventoryItemDto upsertItem);
}
