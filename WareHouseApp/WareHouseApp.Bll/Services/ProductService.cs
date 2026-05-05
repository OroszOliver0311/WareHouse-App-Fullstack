using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
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
                UnitPrice = p.UnitPrice, 
                Stocks = p.InventoryItems.Select(i => new WareHouseDto
                {
                    Id = i.WareHouseId,
                    Name = i.WareHouse.Name,
                    Location = i.WareHouse.Location,
                    Quantity = i.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException("Product", id);

        return product;
    }
    public async Task<ProductDetailDto> CreateProductAsync(CreateProductDto createProduct)
    {
        var product = new Product
        {
            Name = createProduct.Name,
            SKU = createProduct.SKU,
            UnitPrice = createProduct.Price
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return await GetProductDetailAsync(product.Id);
    }
    public async Task<ProductDashboardDto> UpdateProductAsync(int id, UpdateProductDto updateProduct)
    {
        var product = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new EntityNotFoundException("Product", id);
        product.Name = updateProduct.Name;
        product.SKU = updateProduct.SKU;
        product.UnitPrice = updateProduct.Price;
        await context.SaveChangesAsync();
         return await context.Products
        .Where(p => p.Id == product.Id)
        .Select(p => new ProductDashboardDto
        {
            Id = p.Id,
            Name = p.Name,
            SKU = p.SKU,
            TotalQuantity = p.InventoryItems.Sum(i => i.Quantity) 
        })
        .FirstAsync();
    }
    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new EntityNotFoundException("Product", id);
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}
