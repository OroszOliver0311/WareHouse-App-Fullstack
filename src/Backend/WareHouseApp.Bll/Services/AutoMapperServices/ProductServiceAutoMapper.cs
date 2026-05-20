using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.AutoMapperServices;

public class ProductServiceAutoMapper(AppDbContext context, IMapper mapper) : IProductService
{

    public async Task<IEnumerable<ProductDashboardDto>> GetDashboardAsync()
    {
        return await context.Products
                        .ProjectTo<ProductDashboardDto>(mapper.ConfigurationProvider)
                        .ToListAsync();

    }
    public async Task<ProductDetailDto> GetProductDetailAsync(int id)
    {
        return await context.Products
                        .ProjectTo<ProductDetailDto>(mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p=> p.Id == id)
                         ?? throw new EntityNotFoundException("Product", id);
    }

    public async Task<ProductDetailDto> CreateProductAsync(CreateProductDto createProduct) 
    { 
         var p = mapper.Map<Product>(createProduct);
         context.Products.Add(p);
         await context.SaveChangesAsync();
         return await GetProductDetailAsync(p.Id);
    }
    public async Task<ProductDashboardDto> UpdateProductAsync(int id, UpdateProductDto updateProduct) 
    {
        var p = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
                               ?? throw new EntityNotFoundException("Product", id);
        mapper.Map(updateProduct, p);
        await context.SaveChangesAsync();
        return await context.Products
                            .ProjectTo<ProductDashboardDto>(mapper.ConfigurationProvider)
                            .SingleAsync(pr => pr.Id == id);
    }
    public async Task DeleteProductAsync(int id)
    {
        var p = await context.Products.SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new EntityNotFoundException("Product", id);

        context.Products.Remove(p);
        await context.SaveChangesAsync();
    }

}


