using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Dal;

namespace WareHouseApp.Bll.Services.LINQServices;

public class WareHouseServiceLINQ(AppDbContext context) : IWareHouseService
{
    public async Task<IEnumerable<WareHouseDto>> GetAllWareHousesAsync()
    {
        return await context.Warehouses
            .Select(w => new WareHouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Location = w.Location
            })
            .ToListAsync();
    }

    public async Task<WareHouseDto> GetWareHouseByIdAsync(int id)
    {
        var wareHouse = await context.Warehouses.FirstOrDefaultAsync(w => w.Id == id)
            ?? throw new EntityNotFoundException("WareHouse",id);

        return new WareHouseDto
        {
            Id = wareHouse.Id,
            Name = wareHouse.Name,
            Location = wareHouse.Location
        };
    }
    public async Task<WareHouseDto> CreateWareHouseAsync(CreateWareHouseDto createWareHouse)
    {
        var wareHouse = new Warehouse
        {
            Name = createWareHouse.Name,
            Location = createWareHouse.Location
        };
        context.Warehouses.Add(wareHouse);
        await context.SaveChangesAsync();
        return await GetWareHouseByIdAsync(wareHouse.Id);
    }
    public async Task<WareHouseDto> UpdateWareHouseAsync(int id, CreateWareHouseDto updateWareHouse)
    {
        var wareHouse = await context.Warehouses.FindAsync(id)
            ?? throw new EntityNotFoundException("WareHouse", id);
        wareHouse.Name = updateWareHouse.Name;
        wareHouse.Location = updateWareHouse.Location;
        await context.SaveChangesAsync();
        return await GetWareHouseByIdAsync(wareHouse.Id);
    }
    public async Task DeleteWareHouseAsync(int id)
    {
        var warehouse = await context.Warehouses.FirstOrDefaultAsync(w => w.Id == id)
            ?? throw new EntityNotFoundException("WareHouse", id);


        bool hasStock = await context.InventoryItems
            .AnyAsync(i => i.WareHouseId == id && i.Quantity > 0);

        if (hasStock)
           throw new InvalidOperationException("Ezt a raktárat nem lehet törölni, mert még van benne készlet!");
        context.Remove(warehouse);
        await context.SaveChangesAsync();
    }
}
