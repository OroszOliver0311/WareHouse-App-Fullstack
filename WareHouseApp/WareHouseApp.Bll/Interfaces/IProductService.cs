using WareHouseApp.Bll.Dtos;

namespace WareHouseApp.Bll.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDashboardDto>> GetDashboardAsync();
    Task<ProductDetailDto> GetProductDetailAsync(int id);
}
