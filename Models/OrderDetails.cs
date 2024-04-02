

namespace VonnPizzaBackEndService.Models
{
    public class OrderDetails
    {
        public required int OrderDetailID { get; set; }
        public required int OrderID { get; set; }
        public required int PizzaID { get; set; }
        public required int Quantity { get; set; }

    }
}
