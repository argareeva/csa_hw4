namespace orders.Tables;

public class DishDto
{
    public string Name { get; set; }

    public string? Description { get; set; }
    
    public decimal Price { get; set; }

    public int Quantity { get; set; }
}