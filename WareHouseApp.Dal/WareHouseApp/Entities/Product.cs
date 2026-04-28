namespace WareHouse_App.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required decimal Price { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; } = [];

}
