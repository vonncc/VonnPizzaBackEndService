namespace VonnPizzaBackEndService.Models
{
    public class Orders
    {
        public required int order_id { get; set; }
        public required DateTime date { get; set; }
        public required TimeSpan time { get; set; }

    }
}
