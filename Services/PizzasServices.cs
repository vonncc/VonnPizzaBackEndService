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
    public class PizzasServices
    {
        private readonly PizzasDbContext _context;
        private readonly DRYFunctionLibrary _dryFunctionLibrary;

        public PizzasServices(PizzasDbContext context)
        {
            _context = context;
        }

        public List<Pizzas> GetAllPizzas(int limit = 0)
        {
            if (_dryFunctionLibrary.ShouldShowAll(limit))
                return _context.Pizzas.ToList();

            return _context.Pizzas.Take(limit).ToList();

        }

        public Pizzas GetPizzaById(string id)
        {
            return _context.Pizzas.FirstOrDefault(p => p.PizzaID == id);
        }

        public void AddPizza(Pizzas pizza)
        {
            if (pizza == null)
            {
                throw new ArgumentNullException(nameof(pizza));
            }

            _context.Pizzas.Add(pizza);
            _context.SaveChanges();
        }

        public void UpdatePizza(Pizzas pizza)
        {
            _context.Entry(pizza).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeletePizza(int id)
        {
            var pizzaToDelete = _context.Pizzas.Find(id);
            if (pizzaToDelete != null)
            {
                _context.Pizzas.Remove(pizzaToDelete);
                _context.SaveChanges();
            }
        }

        public void SaveInChunks(List<Pizzas> mergedRecords)
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
                    _context.SaveChanges();

                    // Clear the chunk for the next batch
                    chunk.Clear();
                }
                else if (count == mergedRecords.Count)
                {
                    // Save the last chunk to the database
                    _context.Pizzas.AddRange(chunk);
                    _context.SaveChanges();
                }
            }
        }
    }
}
