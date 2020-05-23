using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplePizzaApp.Services
{
    public interface IPizzaService
    {
        List<Pizza> Index();
        Pizza Store(string name, string description, decimal price);
        Pizza Store(string name, string description, decimal price, List<Ingredient> ingredients);
        Pizza Show(int id);
        Pizza Update(int id, Pizza newPizza);
        void Delete(int id);
    }
    public class PizzaService : IPizzaService
    {
        private SimplePizzaAppDbContext context;

        public PizzaService(SimplePizzaAppDbContext context)
        {
            this.context = context;
        }
        public void Delete(int id)
        {
            var pizza = this.context.Pizzas.FirstOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                throw new ArgumentException("Invalid pizza id.", "id");
            }
            this.context.Pizzas.Remove(pizza);
            this.context.SaveChanges();
        }

        public List<Pizza> Index()
        {
            var pizzas = this.context.Pizzas.ToList();

            return pizzas;
        }

        public Pizza Show(int id)
        {
            var pizza = this.context.Pizzas.FirstOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                throw new ArgumentException("Invalid pizza id.", "id");
            }
            return pizza;
        }

        public Pizza Store(string name, string description, decimal price)
        {
            var pizza = new Pizza
            {
                Name = name,
                Description = description,
                Price = price
            };

            this.context.Pizzas.Add(pizza);
            this.context.SaveChanges();

            return pizza;
        }

        public Pizza Store(string name, string description, decimal price, List<Ingredient> ingredientsList)
        {
            var pizza = new Pizza
            {
                Name = name,
                Description = description,
                Price = price
            };

            var ingredients = new List<IngredientPizza>();
            foreach (var ingredient in ingredientsList)
            {
                ingredients.Add(new IngredientPizza() { Ingredient = ingredient, Pizza = pizza });
            }

            pizza.Ingredients.AddRange(ingredients);

            this.context.Pizzas.Add(pizza);
            this.context.SaveChanges();

            return pizza;
        }

        public Pizza Update(int id, Pizza newPizza)
        {
            var pizza = this.context.Pizzas.FirstOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                throw new ArgumentException("Invalid pizza id.", "id");
            }

            pizza.Name = newPizza.Name;
            pizza.Description = newPizza.Description;
            pizza.Price = newPizza.Price;
            pizza.Ingredients = newPizza.Ingredients;
            pizza.UpdatedAt = DateTime.UtcNow;

            this.context.SaveChanges();

            return pizza;
        }
    }
}
