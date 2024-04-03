using Microsoft.EntityFrameworkCore;
using VonnPizzaBackEndService.Models;

namespace VonnPizzaBackEndService.Data
{
    public class OrdersDbContext: DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }
        public DbSet<Orders> Orders { get; set; }
    }
}
