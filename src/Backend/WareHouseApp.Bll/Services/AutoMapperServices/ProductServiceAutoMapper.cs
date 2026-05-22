using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Dtos.Encoding;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.AutoMapperServices;

public class ProductServiceAutoMapper(AppDbContext context, IMapper mapper) : IProductService
{

    public async Task<IEnumerable<ProductDashboardDto>> GetDashboardAsync()
    {
        var products = await context.Products
                    .Include(p => p.InventoryItems) 
                    .ToListAsync();
        return mapper.Map<IEnumerable<ProductDashboardDto>>(products);
    }
    public async Task<ProductDetailDto> GetProductDetailAsync(int id)
    {
        var product = await context.Products
                .Include(p => p.InventoryItems) 
                .SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new EntityNotFoundException("Product", id);

        return mapper.Map<ProductDetailDto>(product);
    }

    public async Task<ProductDetailDto> CreateProductAsync(CreateProductDto createProduct) 
    { 
         var p = mapper.Map<Product>(createProduct);
         context.Products.Add(p);
         await context.SaveChangesAsync();
         return await GetProductDetailAsync(p.Id);
    }
    public async Task<ProductDashboardDto> UpdateProductAsync(int id, CreateProductDto updateProduct) 
    {
        var p = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new EntityNotFoundException("Product", id);

        mapper.Map(updateProduct, p);
        await context.SaveChangesAsync();

        var updatedProduct = await context.Products
                .Include(pr => pr.InventoryItems) 
                .SingleAsync(pr => pr.Id == id);  

        return mapper.Map<ProductDashboardDto>(updatedProduct);
    }
    public async Task DeleteProductAsync(int id)
    {
        var p = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new EntityNotFoundException("Product", id);

        context.Products.Remove(p);
        await context.SaveChangesAsync();
    }

}


