using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebTask.Models;

namespace WebTask.Data
{
    public class ShopperContext : DbContext
    {
        public ShopperContext(DbContextOptions<ShopperContext> options) : base(options) { }

        public DbSet<Shopper> Shoppers { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed ShoppingItem entities
                modelBuilder.Entity<ShoppingItem>().HasData(
                    new ShoppingItem { Id = 1, Name = "Apples" },
                    new ShoppingItem { Id = 2, Name = "Oranges" },
                    new ShoppingItem { Id = 3, Name = "Bananas" },
                    new ShoppingItem { Id = 4, Name = "Pears" },
                    new ShoppingItem { Id = 5, Name = "Grapes" }
                );

            // Seed Shopper entities
           
                modelBuilder.Entity<Shopper>().HasData(
                    new Shopper { Id = 1, Name = "Emma" },
                    new Shopper { Id = 2, Name = "Lucas" },
                    new Shopper { Id = 3, Name = "Olivia" },
                    new Shopper { Id = 4, Name = "Louis" },
                    new Shopper { Id = 5, Name = "Amir" }
                );
            
        }

    }
}
