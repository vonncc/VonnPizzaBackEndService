using Microsoft.EntityFrameworkCore;
using VonnPizzaBackEndService.Data;
using VonnPizzaBackEndService.Models;

namespace VonnPizzaBackEndService.Services
{
    public class OrderDetailsServices
    {
        private readonly OrderDetailsDbContext _context;

        public OrderDetailsServices(OrderDetailsDbContext context)
        {
            _context = context;
        }

        public List<OrderDetails> GetAllOrderDetails()
        {
            return _context.OrderDetails.ToList();
        }

        public OrderDetails GetOrderDetailById(int id)
        {
            return _context.OrderDetails.FirstOrDefault(p => p.OrderID == id);
        }

        public void AddOrderDetails(OrderDetails orderDetails)
        {
            if (orderDetails == null)
            {
                throw new ArgumentNullException(nameof(orderDetails));
            }

            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
        }

        public void UpdateOrderDetails(OrderDetails orderDetails)
        {
            _context.Entry(orderDetails).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteOrderDetails(int id)
        {
            var orderDetailsToDelete = _context.OrderDetails.Find(id);
            if (orderDetailsToDelete != null)
            {
                _context.OrderDetails.Remove(orderDetailsToDelete);
                _context.SaveChanges();
            }
        }

        public void SaveInChunks(List<OrderDetails> processedRecord)
        {
            var batchSize = 1000;
            var count = 0;
            var chunk = new List<OrderDetails>();

            foreach (var record in processedRecord)
            {
                chunk.Add(record);
                count++;

                if (count % batchSize == 0 && count != processedRecord.Count)
                {
                    // Save the chunk to the database
                    _context.OrderDetails.AddRange(chunk);
                    _context.SaveChanges();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == processedRecord.Count)
                {
                    // Save the last chunk to the database
                    _context.OrderDetails.AddRange(chunk);
                    _context.SaveChanges();
                }
            }
        }
    }
}
