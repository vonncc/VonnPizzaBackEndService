using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VonnPizzaBackEndService.Models
{
    public class Pizzas
    {
        [Key] // Define the primary key
        public required string PizzaID { get; set; }
       // public required string PizzaTypeID { get; set; }
        public required string Size { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required double Price { get; set; }
        public required string Ingredients { get; set; }

    }
}
