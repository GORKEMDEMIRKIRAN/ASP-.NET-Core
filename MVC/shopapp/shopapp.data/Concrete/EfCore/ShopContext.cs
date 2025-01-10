

using Microsoft.EntityFrameworkCore;
using shopapp.entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace shopapp.data.Concrete.EfCore
{
    public class ShopContext:DbContext
    {
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shopmarketdb.db");
        }

        // fluetn api bir concsion birincil anahtarları belirtelim.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(c=>new {c.CategoryId,c.ProductId});
        }

    }
}