namespace orders.Tables;

public class OrderDto
{
    public int UserId { get; set; }
    public int DishId { get; set; }
    public string Status { get; set; }
    public string? SpecialRequests { get; set; }
}