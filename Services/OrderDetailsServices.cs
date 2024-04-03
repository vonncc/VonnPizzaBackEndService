using Microsoft.EntityFrameworkCore;
using System.Linq;
using VonnPizzaBackEndService.Data;
using VonnPizzaBackEndService.Models;
using VonnPizzaBackEndService.Utilities;

namespace VonnPizzaBackEndService.Services
{
    public class OrderDetailsServices
    {
        private readonly OrderDetailsDbContext _context;
        private readonly DRYFunctionLibrary _dryFunctionLibrary;

        public OrderDetailsServices(OrderDetailsDbContext context, DRYFunctionLibrary dryFunctionLibrary)
        {
            _context = context;
            _dryFunctionLibrary = dryFunctionLibrary;
        }

        public async Task<List<OrderDetails>> GetAllOrderDetailsAsync(int limit = 0)
        {
            if (_dryFunctionLibrary.ShouldShowAll(limit))
                return await _context.OrderDetails.ToListAsync();

            return await _context.OrderDetails.Take(limit).ToListAsync();
        }

        public async Task<OrderDetails> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(p => p.OrderID == id);
        }

        public async Task AddOrderDetailsAsync(OrderDetails orderDetails)
        {
            if (orderDetails == null)
            {
                throw new ArgumentNullException(nameof(orderDetails));
            }

            _context.OrderDetails.Add(orderDetails);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderDetailsAsync(OrderDetails orderDetails)
        {
            _context.Entry(orderDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailsAsync(int id)
        {
            var orderDetailsToDelete = await _context.OrderDetails.FindAsync(id);
            if (orderDetailsToDelete != null)
            {
                _context.OrderDetails.Remove(orderDetailsToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveInChunksAsync(List<OrderDetails> processedRecord)
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
                    await _context.SaveChangesAsync();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == processedRecord.Count)
                {
                    // Save the last chunk to the database
                    _context.OrderDetails.AddRange(chunk);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
