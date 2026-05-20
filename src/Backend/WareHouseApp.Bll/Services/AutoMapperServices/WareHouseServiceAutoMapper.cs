using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.AutoMapperServices;

public class WareHouseServiceAutoMapper(AppDbContext context, IMapper mapper) : IWareHouseService
{
    public async Task<IEnumerable<WareHouseDto>> GetAllWareHousesAsync()
    {
        return await context.Warehouses
            .ProjectTo<WareHouseDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }


    public async Task<WareHouseDto> GetWareHouseByIdAsync(int id)
    {
        return await context.Warehouses
            .ProjectTo<WareHouseDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(w => w.Id == id)
            ?? throw new EntityNotFoundException("WareHouse", id);

    }
    public async Task<WareHouseDto> CreateWareHouseAsync(CreateWareHouseDto createWareHouse)
    {
        var wareHouse = mapper.Map<Warehouse>(createWareHouse);
        context.Warehouses.Add(wareHouse);
        await context.SaveChangesAsync();
        return await GetWareHouseByIdAsync(wareHouse.Id);
    }
    public async Task<WareHouseDto> UpdateWareHouseAsync(int id, CreateWareHouseDto updateWareHouse)
    {
        var w = await context.Warehouses
            .SingleOrDefaultAsync(w => w.Id == id)
            ?? throw new EntityNotFoundException("WareHouse", id);

        mapper.Map(updateWareHouse, w);
        await context.SaveChangesAsync();
        return await GetWareHouseByIdAsync(id);

    }
    public async Task DeleteWareHouseAsync(int id)
    {
        var wareHouse = await context.Warehouses.SingleOrDefaultAsync(w => w.Id == id)
            ?? throw new EntityNotFoundException("WareHouse", id);
        bool hasStock = await context.InventoryItems
                        .AnyAsync(i => i.WareHouseId == id && i.Quantity > 0);

        if (hasStock)
            throw new InvalidOperationException("Ezt a raktárat nem lehet törölni, mert még van benne készlet!");
        context.Remove(wareHouse);
        await context.SaveChangesAsync();
    }

}
