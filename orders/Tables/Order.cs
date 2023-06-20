using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orders.Tables;

[Table("order")]
public class Order
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("user_id")] 
    public int UserId { get; set; }

    [ForeignKey("UserId")] 
    public User User { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; }

    [Column("special_requests")] 
    public string? SpecialRequests { get; set; }

    [Column("created_at")] 
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")] 
    public DateTime UpdatedAt { get; set; }
}