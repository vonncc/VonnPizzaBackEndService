

namespace VonnPizzaBackEndService.Models
{
    public class Pizzas
    {
        public required int PizzaID { get; set; }
        public required int pizza_type_id { get; set; }
        public required string Size { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required double Price { get; set; }
        public required string Ingredients { get; set; }
    }
}
