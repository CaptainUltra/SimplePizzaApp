using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplePizzaApp.Services.Tests
{
    public class PizzaServiceTests
    {
        private DbContextOptions<SimplePizzaAppDbContext> contextOptions;
        private SimplePizzaAppDbContext context;

        public PizzaServiceTests()
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
        public void PizzaCanBeCreated()
        {
            var service = new PizzaService(context);

            var pizza = service.Store("Pizza 1", "Description", 5.6m);

            Assert.AreEqual("Pizza 1", pizza.Name);
            Assert.AreEqual(1, pizza.Id);
            Assert.AreEqual(1, context.Pizzas.Count());
        }
        [Test]
        public void PizzaCanBeCreatedWithListOfIngredients()
        {
            var ingredient1 = new Ingredient() { Name = "Ingredient1" };
            var ingredient2 = new Ingredient() { Name = "Ingredient2" };
            context.Ingredients.Add(ingredient1);
            context.Ingredients.Add(ingredient2);
            context.SaveChanges();
            var service = new PizzaService(context);
            var ingredients = new List<Ingredient>();
            ingredients.Add(ingredient1);
            ingredients.Add(ingredient2);

            var pizza = service.Store("Pizza 1", "Description", 5.6m, ingredients);

            Assert.AreEqual("Pizza 1", pizza.Name);
            Assert.AreEqual(1, pizza.Id);
            Assert.AreEqual(1, context.Pizzas.Count());
            Assert.AreEqual(2, pizza.Ingredients.Count());
            Assert.AreEqual(2, context.IngredientsPizzas.Count());
        }

        [Test]
        public void PizzaCanBeRetrieved()
        {
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            context.SaveChanges();
            var service = new PizzaService(context);

            var pizza = service.Show(1);

            Assert.AreEqual(1, pizza.Id);
            Assert.AreEqual("Pizza", pizza.Name);
        }
        [Test]
        public void PizzaWithInvalidId_WhenRetrieved_ThrowsExeption()
        {
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            context.SaveChanges();
            var service = new PizzaService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Show(2));
            Assert.That(ex.Message, Is.EqualTo("Invalid pizza id. (Parameter 'id')"));
        }
        [Test]
        public void PizzasListCanBeRetrieved()
        {
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            context.SaveChanges();
            var service = new PizzaService(context);

            var pizzas = service.Index();

            Assert.AreEqual(2, pizzas.Count);
        }
        [Test]
        public void PizzaCanBeUpdated()
        {
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            var ingredient1 = new Ingredient() { Name = "Ingredient1" };
            var ingredient2 = new Ingredient() { Name = "Ingredient2" };
            context.Ingredients.Add(ingredient1);
            context.Ingredients.Add(ingredient2);
            context.SaveChanges();
            var service = new PizzaService(context);

            var updateData = new Pizza { Name = "Pizza2"};
            var ingredients = new List<IngredientPizza>();
            ingredients.Add(new IngredientPizza { Ingredient = ingredient1, Pizza = updateData });
            ingredients.Add(new IngredientPizza { Ingredient = ingredient2, Pizza = updateData });
            updateData.Ingredients = ingredients;

            var pizza = service.Update(1, updateData);
            var pizzaRecord = context.Pizzas.Single(i => i.Name == "Pizza2");

            Assert.AreEqual("Pizza2", pizza.Name);
            Assert.AreEqual("Pizza2", pizzaRecord.Name);
            Assert.AreEqual(2, pizza.Ingredients.Count());
            Assert.AreEqual(2, context.IngredientsPizzas.Count());
        }
        [Test]
        public void PizzaWithInvalidId_WhenUpdated_ThrowsExeption()
        {
            var service = new PizzaService(context);
            var updateData = new Pizza { Name = "Pizza" };

            var ex = Assert.Throws<ArgumentException>(() => service.Update(1, updateData));
            Assert.That(ex.Message, Is.EqualTo("Invalid pizza id. (Parameter 'id')"));
        }
        [Test]
        public void PizzaCanBeDeleted()
        {
            context.Pizzas.Add(new Pizza { Name = "Pizza" });
            context.SaveChanges();
            var service = new PizzaService(context);

            service.Delete(1);

            Assert.AreEqual(0, context.Pizzas.Count());
        }
        [Test]
        public void PizzaWithInvalidId_WhenDeleted_ThrowsExeption()
        {
            var service = new PizzaService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Delete(1));
            Assert.That(ex.Message, Is.EqualTo("Invalid pizza id. (Parameter 'id')"));
        }
    }
}