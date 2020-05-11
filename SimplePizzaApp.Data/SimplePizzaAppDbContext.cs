using Microsoft.EntityFrameworkCore;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Data
{
    public class SimplePizzaAppDbContext :DbContext
    {
        public SimplePizzaAppDbContext(DbContextOptions<SimplePizzaAppDbContext> options)
            : base(options)
        { }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<IngredientPizza> IngredientsPizzas { get; set; }
        public DbSet<OrderPizza> OrdersPizzas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IngredientPizza>()
            .HasKey(x => new { x.IngredientId, x.PizzaId});

            modelBuilder.Entity<IngredientPizza>()
                .HasOne(ip => ip.Ingredient)
                .WithMany(i => i.Pizzas)
                .HasForeignKey(ip => ip.IngredientId);

            modelBuilder.Entity<IngredientPizza>()
                .HasOne(ip => ip.Pizza)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(ip => ip.PizzaId);

            modelBuilder.Entity<OrderPizza>()
            .HasKey(x => new { x.OrderId, x.PizzaId });

            modelBuilder.Entity<OrderPizza>()
                .HasOne(op => op.Order)
                .WithMany(o => o.Pizzas)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderPizza>()
                .HasOne(ip => ip.Pizza)
                .WithMany(o => o.Orders)
                .HasForeignKey(op => op.PizzaId);
        }
    }
}
