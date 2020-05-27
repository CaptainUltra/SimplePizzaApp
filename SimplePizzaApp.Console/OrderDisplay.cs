using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleOrderApp.Console
{
    /// <summary>
    ///  Handles console input/output for Order and communicates to the service.
    /// </summary>
    class OrderDisplay
    {
        private IOrderService service;
        private IPizzaService pizzaService;
        /// <summary>
        ///  Initialize the class.
        /// </summary>
        /// <param name="service">Used to communicate to the business logic layer.</param>
        public OrderDisplay(IOrderService service, IPizzaService pizzaService)
        {
            this.service = service;
            this.pizzaService = pizzaService;
        }

        /// <summary>
        ///  Handles console IO for creating a record and passes the data to the service.
        /// </summary>
        public void Create()
        {
            System.Console.WriteLine("Добавяне на поръчка:");
            System.Console.Write("Име на клиент: ");
            var name = System.Console.ReadLine();
            System.Console.Write("Адрес на клиент: ");
            var address = System.Console.ReadLine();

            System.Console.WriteLine("Напишете номерата на пиците, които искате да добавите, разделени с интервал: ");
            var pizzaIds = System.Console.ReadLine().Split().Select(int.Parse);
            List<Pizza> pizzas = new List<Pizza>();
            foreach (var id in pizzaIds)
            {
                var pizza = this.pizzaService.Show(id);
                pizzas.Add(pizza);
            }
            this.service.Store(name, address, pizzas);
            System.Console.WriteLine("Успешно добавяне на поръчка!");
        }
        /// <summary>
        ///  Retrieves data from the service and outputs it.
        /// </summary>
        public void List()
        {
            System.Console.WriteLine("Всички поръчки:");
            var orders = this.service.Index();
            foreach (var order in orders)
            {
                System.Console.WriteLine($"№: {order.Id} Име на клиент: {order.ClientName} Адрес: {order.Address} Цена: {order.Total:F2}");
            }
        }
        /// <summary>
        ///  Handles console IO and gets data from the service for retrieving a record.
        /// </summary>
        public void Show()
        {
            System.Console.WriteLine("Извеждане на поръчка:");
            System.Console.Write("Номер на поръчката: ");
            var id = int.Parse(System.Console.ReadLine());
            var order = this.service.Show(id);
            System.Console.WriteLine($"№: {order.Id} Име на клиент: {order.ClientName} Адрес: {order.Address} Цена: {order.Total:F2}");
            System.Console.WriteLine("Пици:");
            foreach (var pizza in order.Pizzas)
            {
                System.Console.WriteLine(pizza.Pizza.Name);
            }
        }
        /// <summary>
        ///  Handles console IO for updating a record and passes the data to the service.
        /// </summary>
        public void Update()
        {
            System.Console.WriteLine("Промяна на поръчка:");
            System.Console.Write("Номер на поръчката: ");
            var id = int.Parse(System.Console.ReadLine());
            System.Console.Write("Ново име на клиент: ");
            var name = System.Console.ReadLine();
            System.Console.Write("Адрес: ");
            var address = System.Console.ReadLine();

            System.Console.WriteLine("Промяна на пици? (1 - да; 0 - не)");
            var option = int.Parse(System.Console.ReadLine());
            if (option == 0)
            {
                var existingPizzas = this.service.Show(id).Pizzas;
                var updateOrder = new Order { ClientName = name, Address = address, Pizzas = existingPizzas };
                this.service.Update(id, updateOrder);
                System.Console.WriteLine("Успешна промяна на поръчката!");
                return;
            }

            System.Console.WriteLine("Напишете номерата на пиците, които искате да добавите, разделени с интервал: ");
            var pizzaIds = System.Console.ReadLine().Split().Select(int.Parse);
            List<OrderPizza> pizzas = new List<OrderPizza>();
            var order = new Order { ClientName = name, Address = address};
            foreach (var pizzaId in pizzaIds)
            {
                var pizza = this.pizzaService.Show(pizzaId);
                pizzas.Add(new OrderPizza { Order = order, Pizza = pizza });
            }

            order.Pizzas = pizzas;

            this.service.Update(id, order);
            System.Console.WriteLine("Успешна промяна на поръчката!");
        }
        /// <summary>
        ///  Handles console IO for deleting a record and passes the data to the service.
        /// </summary>
        public void Delete()
        {
            System.Console.WriteLine("Изтриване на поръчка:");
            System.Console.Write("Номер на поръчката: ");
            var id = int.Parse(System.Console.ReadLine());
            this.service.Delete(id);
            System.Console.WriteLine("Успешно изтривнае на поръчката!");
        }
    }
}
