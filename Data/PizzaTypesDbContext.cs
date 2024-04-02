using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;

namespace VonnPizzaBackEndService.Data
{
    public class PizzaTypesDbContext : DbContext
    {
        public PizzaTypesDbContext(DbContextOptions<PizzaTypesDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
