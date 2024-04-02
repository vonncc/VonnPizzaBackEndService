using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Configuration;

public class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<MyEntity> MyEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }
}

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    // Other properties
}


namespace VonnPizzaBackEndService.Utilities
{
    public class CSVImporter
    {
        private readonly MyDbContext _dbContext;

        public CSVImporter(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ImportCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var entities = new List<MyEntity>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var entity = new MyEntity { Name = values[0] }; // Assuming CSV has one column for the name
                    entities.Add(entity);

                    if (entities.Count % 1000 == 0)
                    {
                        _dbContext.MyEntities.AddRange(entities);
                        _dbContext.SaveChanges();
                        entities.Clear();
                    }
                }

                if (entities.Any())
                {
                    _dbContext.MyEntities.AddRange(entities);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
