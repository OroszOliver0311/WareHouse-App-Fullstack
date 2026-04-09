namespace WareHouse_App.Entities;

public class Warehouse
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public required string Location { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; } = [];
}
