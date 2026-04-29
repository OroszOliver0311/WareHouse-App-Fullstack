using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
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
    public async Task<ProductDetailDto> GetProductDetailAsync(int id)
    {
        var product = await context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                UnitPrice = p.Price, 
                Stocks = p.InventoryItems.Select(i => new WareHouseDto
                {
                    Id = i.WarehouseId,
                    Name = i.Warehouse.Name,
                    Location = i.Warehouse.Location,
                    Quantity = i.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (product == null)
            throw new EntityNotFoundException("Product", id);

        return product;
    }
}
