using WareHouseApp.Bll.Dtos;

namespace WareHouseApp.Bll.Interfaces;

public interface IStockMovementService
{
    Task<IEnumerable<StockMovementDto>> GetProductHistoryAsync(int productId);
}
