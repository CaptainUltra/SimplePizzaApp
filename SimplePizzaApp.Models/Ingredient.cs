using System;
using System.Collections.Generic;

namespace SimplePizzaApp.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<IngredientPizza> Pizzas { get; set; }
    }
}
