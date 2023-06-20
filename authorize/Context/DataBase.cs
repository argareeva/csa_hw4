using Microsoft.EntityFrameworkCore;
using authorize.Tables;

namespace authorize.Context
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options)
            : base(options)
        {

        }
        public DbSet<User>? User { get; set; }
        public DbSet<Session>? Session { get; set; }
    }
}