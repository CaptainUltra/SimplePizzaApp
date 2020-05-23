using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplePizzaApp.Services.Tests
{
    class OrderServiceTests
    {
        private DbContextOptions<SimplePizzaAppDbContext> contextOptions;
        private SimplePizzaAppDbContext context;

        public OrderServiceTests()
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
        public void OrderCanBeCreated()
        {
            var pizza1 = new Pizza() { Name = "Pizza1", Price = 1.2m };
            var pizza2 = new Pizza() { Name = "Pizza2", Price = 1.3m };
            context.Pizzas.Add(pizza1);
            context.Pizzas.Add(pizza2);
            context.SaveChanges();
            var service = new OrderService(context);
            var pizzas = new List<Pizza>();
            pizzas.Add(pizza1);
            pizzas.Add(pizza2);
            var total = pizza1.Price + pizza2.Price;

            var order = service.Store("John Doe", "some street address", pizzas);

            Assert.AreEqual("John Doe", order.ClientName);
            Assert.AreEqual(1, order.Id);
            Assert.AreEqual(1, context.Orders.Count());
            Assert.AreEqual(2, order.Pizzas.Count());
            Assert.AreEqual(2, context.OrdersPizzas.Count());
            Assert.AreEqual(total, order.Total);
        }

        [Test]
        public void OrderCanBeRetrieved()
        {
            context.Orders.Add(new Order { ClientName = "John Doe", Address =  "some street address"});
            context.SaveChanges();
            var service = new OrderService(context);

            var order = service.Show(1);

            Assert.AreEqual(1, order.Id);
            Assert.AreEqual("John Doe", order.ClientName);
        }
        [Test]
        public void OrderWithInvalidId_WhenRetrieved_ThrowsExeption()
        {
            context.Orders.Add(new Order { ClientName = "John Doe", Address = "some street address" });
            context.SaveChanges();
            var service = new OrderService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Show(2));
            Assert.That(ex.Message, Is.EqualTo("Invalid order id. (Parameter 'id')"));
        }
        [Test]
        public void OrdersListCanBeRetrieved()
        {
            context.Orders.Add(new Order { ClientName = "John Doe", Address = "some street address" });
            context.Orders.Add(new Order { ClientName = "John Doe2", Address = "some street address2" });
            context.SaveChanges();
            var service = new OrderService(context);

            var orders = service.Index();

            Assert.AreEqual(2, orders.Count);
        }
        [Test]
        public void OrderCanBeUpdated()
        {
            context.Orders.Add(new Order { ClientName = "John Doe", Address = "some street address" });
            var pizza1 = new Pizza() { Name = "Pizza1" };
            var pizza2 = new Pizza() { Name = "Pizza2" };
            context.Pizzas.Add(pizza1);
            context.Pizzas.Add(pizza2);
            context.SaveChanges();
            var service = new OrderService(context);

            var updateData = new Order { ClientName = "John Doe1", Address = "some street address2" };
            var pizas = new List<OrderPizza>();
            pizas.Add(new OrderPizza { Pizza = pizza1, Order = updateData });
            pizas.Add(new OrderPizza { Pizza = pizza2, Order = updateData });
            updateData.Pizzas = pizas;

            var pizza = service.Update(1, updateData);
            var pizzaRecord = context.Orders.Single(i => i.ClientName == "John Doe1");

            Assert.AreEqual("John Doe1", pizza.ClientName);
            Assert.AreEqual("John Doe1", pizzaRecord.ClientName);
            Assert.AreEqual(2, pizza.Pizzas.Count());
            Assert.AreEqual(2, context.OrdersPizzas.Count());
        }
        [Test]
        public void OrderWithInvalidId_WhenUpdated_ThrowsExeption()
        {
            var service = new OrderService(context);
            var updateData = new Order { ClientName = "John Doe", Address = "some street address" };

            var ex = Assert.Throws<ArgumentException>(() => service.Update(1, updateData));
            Assert.That(ex.Message, Is.EqualTo("Invalid order id. (Parameter 'id')"));
        }
        [Test]
        public void OrderCanBeDeleted()
        {
            context.Orders.Add(new Order { ClientName = "John Doe", Address = "some street address" });
            context.SaveChanges();
            var service = new OrderService(context);

            service.Delete(1);

            Assert.AreEqual(0, context.Orders.Count());
        }
        [Test]
        public void OrderWithInvalidId_WhenDeleted_ThrowsExeption()
        {
            var service = new OrderService(context);

            var ex = Assert.Throws<ArgumentException>(() => service.Delete(1));
            Assert.That(ex.Message, Is.EqualTo("Invalid order id. (Parameter 'id')"));
        }
    }
}
