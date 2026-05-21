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
    public decimal UnitPrice { get; init; } = 0;

}
public record ProductDetailDto 
{ 
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public decimal UnitPrice { get; init; } = 0;
    public IEnumerable<ProductDetailWareHouseDto> Stocks { get; init; } = [];

}
public record InventoryItemDto
{
    public int ProductId { get; init; }
    public int WareHouseId { get; init; }
    public int Quantity { get; init; }
}
public record ProductDetailWareHouseDto
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public string Location { get; init; } = null!;

    public int Quantity { get; init; }
}
public record WareHouseDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string Location { get; init; } = null!;
}
public record CreateWareHouseDto
{
    public string? Name { get; init; }
    public string Location { get; init; } = null!;
}
public record StockMovementDto
{
    public int Id { get; init; }
    public string WareHouseLocation { get; init; } = null!;
    public bool IsIncoming { get; init; }
    public int Quantity { get; init; }
    public DateTimeOffset Date { get; init; }
}
