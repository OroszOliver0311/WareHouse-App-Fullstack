using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseApp.Bll.Dtos;

public record ProductDashboardDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public int TotalQuantity { get; init; }
}
public record CreateProductDto
{
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public decimal Price { get; init; } = 0;

}
public record ProductDetailDto 
{ 
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public decimal UnitPrice { get; init; } = 0;
    public IEnumerable<WareHouseDto> Stocks { get; init; } = [];

}
public record UpdateProductDto
{
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public decimal Price { get; init; } = 0;

}
public record InventoryItemDto
{
    public int ProductId { get; init; }
    public int WareHouseId { get; init; }
    public int Quantity { get; init; }
}

public record WareHouseDto
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public string Location { get; init; } = null!;

    public int Quantity { get; init; }
}