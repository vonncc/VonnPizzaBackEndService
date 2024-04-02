namespace VonnPizzaBackEndService.Models
{
    public class OrderDetails
    {
        public required int order_details_id { get; set; }
        public required int order_id { get; set; }
        public required int pizza_id { get; set; }
        public required int quantity { get; set; }
    }
}
