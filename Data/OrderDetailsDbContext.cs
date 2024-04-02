using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using VonnPizzaBackEndService.Models;

namespace VonnPizzaBackEndService.Data
{
    public class OrderDetailsDbContext : DbContext
    {
        public OrderDetailsDbContext(DbContextOptions<OrderDetailsDbContext> options) : base(options)
        {
        }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
