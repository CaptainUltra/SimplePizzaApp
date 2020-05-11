using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Models
{
    public class Pizza
    {
        public Pizza()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.Ingredients = new List<IngredientPizza>();
            this.Orders = new List<OrderPizza>();
        }
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<IngredientPizza> Ingredients { get; set; }
        public List<OrderPizza> Orders { get; set; }
    }
}
