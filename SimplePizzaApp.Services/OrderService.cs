using SimplePizzaApp.Data;
using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePizzaApp.Services
{
    public interface IOrderService
    {
        List<Order> Index();
        Order Store(string clientName, string address, List<Pizza> pizzas);
        Order Show(int id);
        Order Update(int id, Order newOrder);
        void Delete(int id);
    }
    public class OrderService : IOrderService
    {
        private SimplePizzaAppDbContext context;

        public OrderService(SimplePizzaAppDbContext context)
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new ArgumentException("Invalid order id.", "id");
            }
            this.context.Orders.Remove(order);
            this.context.SaveChanges();
        }

        public List<Order> Index()
        {
            var orders = this.context.Orders.ToList();

            return orders;
        }

        public Order Show(int id)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new ArgumentException("Invalid order id.", "id");
            }
            return order;
        }

        public Order Store(string clientName, string address, List<Pizza> pizzasList)
        {
            var order = new Order
            {
                ClientName = clientName,
                Address = address
            };

            var pizzas = new List<OrderPizza>();
            foreach (var pizza in pizzasList)
            {
                pizzas.Add(new OrderPizza() { Order = order, Pizza = pizza });
            }

            order.Pizzas.AddRange(pizzas);
            order.Total = order.Pizzas.Sum(x => x.Pizza.Price);

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order;
        }

        public Order Update(int id, Order newOrder)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new ArgumentException("Invalid order id.", "id");
            }

            order.ClientName = newOrder.ClientName;
            order.Address = newOrder.Address;
            order.Pizzas = newOrder.Pizzas;
            order.Total = order.Pizzas.Sum(x => x.Pizza.Price);
            order.UpdatedAt = DateTime.UtcNow;

            this.context.SaveChanges();

            return order;
        }
    }
}
