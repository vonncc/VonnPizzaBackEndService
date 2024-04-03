using Microsoft.EntityFrameworkCore;
using VonnPizzaBackEndService.Data;
using VonnPizzaBackEndService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VonnPizzaBackEndService.Utilities;
using System.Threading.Tasks;


namespace VonnPizzaBackEndService.Services
{
    public class PizzasServices
    {
        private readonly PizzasDbContext _context;
        private readonly DRYFunctionLibrary _dryFunctionLibrary;

        public PizzasServices(PizzasDbContext context, DRYFunctionLibrary dryFunctionLibrary)
        {
            _context = context;
            _dryFunctionLibrary = dryFunctionLibrary;
        }

        public async Task<List<Pizzas>> GetAllPizzasAsync(int limit = 0)
        {
            if (_dryFunctionLibrary.ShouldShowAll(limit))
                return await _context.Pizzas.ToListAsync();

            return await _context.Pizzas.Take(limit).ToListAsync();

        }

        public async Task<Pizzas> GetPizzaByIdAsync(string id)
        {
            return await _context.Pizzas.FirstOrDefaultAsync(p => p.PizzaID == id);
        }

        public async Task AddPizzaAsync(Pizzas pizza)
        {
            if (pizza == null)
            {
                throw new ArgumentNullException(nameof(pizza));
            }

            await _context.Pizzas.AddAsync(pizza);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePizzaAsync(Pizzas pizza)
        {
            _context.Entry(pizza).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePizzaAsync(int id)
        {
            var pizzaToDelete = await _context.Pizzas.FindAsync(id);
            if (pizzaToDelete != null)
            {
                _context.Pizzas.Remove(pizzaToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveInChunksAsync(List<Pizzas> mergedRecords)
        {
            var batchSize = 1000;
            var count = 0;
            var chunk = new List<Pizzas>();

            foreach (var record in mergedRecords)
            {
                chunk.Add(record);
                count++;

                if (count % batchSize == 0 && count != mergedRecords.Count)
                {
                    // Save the chunk to the database
                    _context.Pizzas.AddRange(chunk);
                    await _context.SaveChangesAsync();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == mergedRecords.Count)
                {
                    // Save the last chunk to the database
                    _context.Pizzas.AddRange(chunk);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
