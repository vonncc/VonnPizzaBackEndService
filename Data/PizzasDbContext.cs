using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;

namespace VonnPizzaBackEndService.Data
{
    public class PizzasDbContext : DbContext
    {
        public PizzasDbContext(DbContextOptions<PizzasDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
