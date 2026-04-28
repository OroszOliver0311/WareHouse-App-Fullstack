namespace WareHouse_App.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public int InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = null!;

    public bool IsIncoming { get; set; }
    public required int Quantity { get; set; }
    public required DateTime MovementDate { get; set; }

}
