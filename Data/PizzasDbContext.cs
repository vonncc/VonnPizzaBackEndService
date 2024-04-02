using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using VonnPizzaBackEndService.Models;

namespace VonnPizzaBackEndService.Data
{
    public class PizzasDbContext : DbContext
    {
        public PizzasDbContext(DbContextOptions<PizzasDbContext> options) : base(options)
        {
        }

        public DbSet<Pizzas> Pizzas { get; set; }
    }
}
