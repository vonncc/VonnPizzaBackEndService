using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;

namespace VonnPizzaBackEndService.Data
{
    public class OrderDetailsDbContext : DbContext
    {
        public OrderDetailsDbContext(DbContextOptions<OrderDetailsDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
