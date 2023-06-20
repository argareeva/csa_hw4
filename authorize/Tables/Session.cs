using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authorize.Tables;

[Table("session")]
public class Session
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [Column("session_token")]
    [MaxLength(500)]
    public string SessionToken { get; set; }

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }
}