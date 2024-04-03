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

        public OrdersServices(OrdersDbContext context)
        {
            _context = context;
        }

        public List<Orders> GetAllOrders(int limit = 0)
        {
            if (_dryFunctionLibrary.ShouldShowAll(limit))
                return _context.Orders.ToList();

            return _context.Orders.Take(limit).ToList();
        }

        public Orders GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(p => p.OrderID == id);
        }

        public void AddOrder(Orders orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            _context.Orders.Add(orders);
            _context.SaveChanges();
        }

        public void UpdateOrder(Orders orders)
        {
            _context.Entry(orders).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var pizzaToDelete = _context.Orders.Find(id);
            if (pizzaToDelete != null)
            {
                _context.Orders.Remove(pizzaToDelete);
                _context.SaveChanges();
            }
        }

        public void SaveInChunks(List<Orders> processedRecord)
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
                    _context.SaveChanges();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == processedRecord.Count)
                {
                    // Save the last chunk to the database
                    _context.Orders.AddRange(chunk);
                    _context.SaveChanges();
                }
            }
        }
    }
}
