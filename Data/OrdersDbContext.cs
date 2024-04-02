﻿using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;

namespace VonnPizzaBackEndService.Data
{
    public class OrdersDbContext: DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
