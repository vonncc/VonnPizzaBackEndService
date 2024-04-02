namespace VonnPizzaBackEndService.Models
{
    public class PizzaTypes
    {
        public required int pizza_type_id { get; set; }
        public required string name { get; set; }
        public required string category { get; set; }
        public required string ingredients { get; set; }
    }
}
