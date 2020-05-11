using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Models
{
    public class IngredientPizza
    {
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }
    }
}
