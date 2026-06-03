using WareHouseApp.Bll.Dtos;

namespace WareHouseApp.Bll.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDashboardDto>> GetDashboardAsync();
    Task<ProductDetailDto> GetProductDetailAsync(int id);
    Task<ProductDetailDto> CreateProductAsync(CreateProductDto createProduct);
    Task<ProductDashboardDto> UpdateProductAsync(int id, CreateProductDto updateProduct);
    Task DeleteProductAsync(int id);
}
