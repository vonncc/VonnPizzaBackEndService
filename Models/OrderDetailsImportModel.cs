namespace VonnPizzaBackEndService.Models
{
    public class OrderDetailsImportModel
    {

        public int order_id { get; set; }
        public int order_details_id { get; set; }
        public string pizza_id { get; set; }
        public int quantity { get; set; }
    }
}
