using Microsoft.EntityFrameworkCore;
using VonnPizzaBackEndService.Data;
using VonnPizzaBackEndService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VonnPizzaBackEndService.Utilities;


namespace VonnPizzaBackEndService.Services
{
    public class OrdersServices
    {
        private readonly OrdersDbContext _context;
        private readonly DRYFunctionLibrary _dryFunctionLibrary;

        public OrdersServices(OrdersDbContext context, DRYFunctionLibrary dryFunctionLibrary)
        {
            _context = context;
            _dryFunctionLibrary = dryFunctionLibrary;
        }

        public async Task<List<Orders>> GetAllOrdersAsync(int limit = 0)
        {
            if (_dryFunctionLibrary.ShouldShowAll(limit))
                return await _context.Orders.ToListAsync();

            return await _context.Orders.Take(limit).ToListAsync();
        }

        public async Task<Orders> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.OrderID == id);
        }

        public async Task AddOrderAsync(Orders orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            await _context.Orders.AddAsync(orders);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Orders orders)
        {
            _context.Entry(orders).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var orderToDelete = await _context.Orders.FindAsync(id);
            if (orderToDelete != null)
            {
                _context.Orders.Remove(orderToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveInChunksAsync(List<Orders> processedRecord)
        {
            var batchSize = 1000;
            var count = 0;
            var chunk = new List<Orders>();

            foreach (var record in processedRecord)
            {
                chunk.Add(record);
                count++;

                if (count % batchSize == 0 && count != processedRecord.Count)
                {
                    // Save the chunk to the database
                    _context.Orders.AddRange(chunk);
                    await _context.SaveChangesAsync();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == processedRecord.Count)
                {
                    // Save the last chunk to the database
                    _context.Orders.AddRange(chunk);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
