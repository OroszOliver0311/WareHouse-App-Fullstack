namespace WareHouse_App.DTO;

public class ProductDashboardDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU{ get; set; }
    public required decimal Price { get; set; }
    public int TotalStock { get; set; }
}
