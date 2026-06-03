namespace WareHouse_App.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public required int Quantity { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int WareHouseId { get; set; }
    public Warehouse WareHouse { get; set; } = null!;

    public ICollection<StockMovement> Movements { get; } = [];


}
