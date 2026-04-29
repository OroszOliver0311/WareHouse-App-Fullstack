using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public async Task<IEnumerable<ProductDashboardDto>> GetDashboardAsync()
    {
        var products = await context.Products
                .Select(p => new ProductDashboardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    TotalQuantity = p.InventoryItems.Sum(i => i.Quantity)
                })
                .ToListAsync();
        return products;
    }

}
