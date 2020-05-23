using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Linq;

namespace SimplePizzaApp.Services.Tests
{
    public class IngredientServiceTests
    {
        private DbContextOptions<SimplePizzaAppDbContext> contextOptions;
        private SimplePizzaAppDbContext context;

        public IngredientServiceTests()
        {
            this.contextOptions = new DbContextOptionsBuilder<SimplePizzaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            this.context = new SimplePizzaAppDbContext(this.contextOptions);
        }

        [TearDown]
        public void CleanUpDatabase()
        {
            this.context.Database.EnsureDeleted();
            this.context = null;
        }
        [Test]
        public void IngredientCanBeCreated()
        {
            var service = new IngredientService(context);

            var ingredient = service.Store("Ingredient 1");

            Assert.AreEqual("Ingredient 1", ingredient.Name);
            Assert.AreEqual(1, ingredient.Id);
            Assert.AreEqual(1, context.Ingredients.Count());
        }

        [Test]
        public void IngredientCanBeRetrieved()
        {
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.SaveChanges();
            var service = new IngredientService(context);

            var ingredient = service.Show(1);

            Assert.AreEqual(1, ingredient.Id);
            Assert.AreEqual("Ingredient", ingredient.Name);
        }
        [Test]
        public void IngredientWithInvalidId_WhenRetrieved_ThrowsExeption()
        {
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.SaveChanges();
            var service = new IngredientService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Show(2));
            Assert.That(ex.Message, Is.EqualTo("Invalid ingredient id. (Parameter 'id')"));
        }
        [Test]
        public void IngredientsListCanBeRetrieved()
        {
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.SaveChanges();
            var service = new IngredientService(context);

            var ingredients = service.Index();

            Assert.AreEqual(2, ingredients.Count);
        }
        [Test]
        public void IngredientCanBeUpdated()
        {
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.SaveChanges();
            var service = new IngredientService(context);
            var updateData = new Ingredient { Name = "Ingredient2" };

            var ingredient = service.Update(1, updateData);
            var ingredientRecord = context.Ingredients.Single(i => i.Name == "Ingredient2");

            Assert.AreEqual("Ingredient2", ingredient.Name);
            Assert.AreEqual("Ingredient2", ingredientRecord.Name);
        }
        [Test]
        public void IngredientWithInvalidId_WhenUpdated_ThrowsExeption()
        {
            var service = new IngredientService(context);
            var updateData = new Ingredient { Name = "Ingredient" };

            var ex = Assert.Throws<ArgumentException>(() => service.Update(1, updateData));
            Assert.That(ex.Message, Is.EqualTo("Invalid ingredient id. (Parameter 'id')"));
        }
        [Test]
        public void IngredientCanBeDeleted()
        {
            context.Ingredients.Add(new Ingredient { Name = "Ingredient" });
            context.SaveChanges();
            var service = new IngredientService(context);

            service.Delete(1);

            Assert.AreEqual(0, context.Ingredients.Count());
        }
        [Test]
        public void IngredientWithInvalidId_WhenDeleted_ThrowsExeption()
        { 
            var service = new IngredientService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Delete(1));
            Assert.That(ex.Message, Is.EqualTo("Invalid ingredient id. (Parameter 'id')"));
        }
    }
}