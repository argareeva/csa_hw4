using orders.Tables;
using Microsoft.EntityFrameworkCore;
namespace orders.Context;

public class DataBase : DbContext
{
    public DataBase(DbContextOptions<DataBase> options)
        : base(options)
    {

    }
    public DbSet<User>? User { get; set; }
    public DbSet<Order>? Order { get; set; }
    public DbSet<Dish>? Dish { get; set; }
    public DbSet<DishOrder>? DishOrder { get; set; }
}