namespace VonnPizzaBackEndService.Models
{
    public class PizzaTypeImportModel
    {
        public required string pizza_type_id { get; set; }
        public required string name { get; set; }
        public required string category { get; set; }
        public required string ingredients { get; set; }
    }

}
