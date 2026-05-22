using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WareHouseApp.Bll.Dtos;

public record ProductDashboardDto
{
    public required string Id { get; init; }
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public int TotalQuantity { get; init; }
}
public record CreateProductDto
{
    [Required]
    public string Name { get; init; } = null!;
    [Required]
    public string SKU { get; init; } = null!;
    public decimal UnitPrice { get; init; } = 0;

}
public record ProductDetailDto 
{ 
    public required string Id { get; init; }
    public string Name { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public decimal UnitPrice { get; init; } = 0;
    public IEnumerable<ProductDetailWareHouseDto> Stocks { get; init; } = [];

}
public record InventoryItemDto
{
    public required string ProductId { get; init; }
    public required string WareHouseId { get; init; }
    public int Quantity { get; init; }
}
public record ProductDetailWareHouseDto
{
    public required string Id { get; init; }

    public string? Name { get; init; }

    public string Location { get; init; } = null!;

    public int Quantity { get; init; }
}
public record WareHouseDto
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public string Location { get; init; } = null!;
}
public record CreateWareHouseDto
{
    public string? Name { get; init; }
    [Required]
    public string Location { get; init; } = null!;
}
public record StockMovementDto
{
    public required string Id { get; init; }
    public string WareHouseLocation { get; init; } = null!;
    public bool IsIncoming { get; init; }
    public int Quantity { get; init; }
    public DateTimeOffset Date { get; init; }
}

//PAGINATION
public record PagedResponse<T>(
    IEnumerable<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);