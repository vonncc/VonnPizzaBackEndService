using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Configuration;

using VonnPizzaBackEndService.Models;

public class MyDbContext<T> : DbContext where T : class
{
    private readonly IConfiguration _configuration;
    private readonly Type _entityType;

    public DbSet<T> Entities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }
}




namespace VonnPizzaBackEndService.Utilities
{

    public class CSVImporter
    {
        private readonly DbContext _dbContext;
        private readonly Type _entityType;

        public CSVImporter()
        {
           
        }

        
    }   

}
