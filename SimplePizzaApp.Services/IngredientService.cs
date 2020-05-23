using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePizzaApp.Services
{
    public interface IIngredientService
    {
        List<Ingredient> Index();
        Ingredient Store(string name);
        Ingredient Show(int id);
        Ingredient Update(int id, Ingredient newIngredient);
        void Delete(int id);
    }
    public class IngredientService : IIngredientService
    {
        private SimplePizzaAppDbContext context;

        public IngredientService(SimplePizzaAppDbContext context)
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            var ingredient = this.context.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient == null)
            {
                throw new ArgumentException("Invalid ingredient id.", "id");
            }
            this.context.Ingredients.Remove(ingredient);
            this.context.SaveChanges();
        }

        public List<Ingredient> Index()
        {
            var list = this.context.Ingredients.ToList();

            return list;
        }

        public Ingredient Show(int id)
        {
            var ingredient = this.context.Ingredients.FirstOrDefault(i => i.Id == id);
            if(ingredient == null)
            {
                throw new ArgumentException("Invalid ingredient id.", "id");
            }
            return ingredient;
        }

        public Ingredient Store(string name)
        {
            var ingredient = new Ingredient
            { 
                Name = name
            };

            this.context.Ingredients.Add(ingredient);
            this.context.SaveChanges();

            return ingredient;
        }

        public Ingredient Update(int id, Ingredient newIngredient)
        {
            var ingredient = this.context.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient == null)
            {
                throw new ArgumentException("Invalid ingredient id.", "id");
            }

            ingredient.Name = newIngredient.Name;
            ingredient.UpdatedAt = DateTime.UtcNow;

            this.context.SaveChanges();

            return ingredient;
        }
    }
}
