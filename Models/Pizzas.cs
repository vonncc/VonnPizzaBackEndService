namespace VonnPizzaBackEndService.Models
{
    public class Pizzas
    {
        public required int pizza_id { get; set; }
        public required int pizza_type_id { get; set; }
        public required string size { get; set; }
        public required double price { get; set; }
    }
}
