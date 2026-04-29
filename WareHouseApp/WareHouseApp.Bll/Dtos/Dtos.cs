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