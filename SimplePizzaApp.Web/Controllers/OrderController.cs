using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using SimplePizzaApp.Models;
using SimplePizzaApp.Services;
using SimplePizzaApp.Web.Models.Order;

namespace SimplePizzaApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService service;
        private IPizzaService pizzaService;

        public OrderController(IOrderService service, IPizzaService pizzaService)
        {
            this.service = service;
            this.pizzaService = pizzaService;
        }
        // GET: Order
        public ActionResult Index()
        {
            var ordersList = this.service.Index();
            var orders = new List<IndexOrderViewModel>();
            foreach (var order in ordersList)
            {
                orders.Add(new IndexOrderViewModel
                {
                    Id = order.Id,
                    ClientName = order.ClientName,
                    Address = order.Address,
                    Total = order.Total,
                    UpdatedAt = order.UpdatedAt
                });
            }

            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var order = this.service.Show(id);
            var orderModel = new ShowOrderViewModel
            {
                Id = order.Id,
                ClientName = order.ClientName,
                Address = order.Address,
                Total = order.Total,
                Pizzas = order.Pizzas,
                UpdatedAt = order.UpdatedAt
            };

            return View(orderModel);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            var pizzas = this.pizzaService.Index();
            ViewBag.Pizzas = pizzas;

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var pizzas = new List<Pizza>();

                if (collection["selectedPizzas"].Count != 0)
                {
                    foreach (var pizza in collection["selectedPizzas"])
                    {
                        var id = int.Parse(pizza);
                        pizzas.Add(this.pizzaService.Show(id));
                    }
                }

                this.service.Store(collection["clientName"], collection["address"], pizzas);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            var order = this.service.Show(id);
            PopulateSelectedPizzas(order);
            var orderModel = new EditOrderViewModel
            {
                ClientName = order.ClientName,
                Address = order.Address
            };
            return View(orderModel);
        }

        /// <summary>
        /// Populates a list of SelectedOrderPizzaViewModel and puts it into the ViewBag. The list contains data about order's selected pizzas.
        /// </summary>
        /// <param name="pizza"></param>
        private void PopulateSelectedPizzas(Order order)
        {
            var allPizzas = this.pizzaService.Index();
            var orderPizzas = new HashSet<int>(order.Pizzas.Select(p => p.PizzaId));
            var viewModel = new List<SelectedOrderPizzaViewModel>();
            foreach (var pizza in allPizzas)
            {
                viewModel.Add(new SelectedOrderPizzaViewModel
                {
                    Id = pizza.Id,
                    Name = pizza.Name,
                    Selected = orderPizzas.Contains(pizza.Id)
                });
            }
            ViewBag.Pizzas = viewModel;
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var pizzas = new List<OrderPizza>();
                var order = new Order { ClientName = collection["clientName"], Address = collection["Address"]};

                if (collection["selectedPizzas"].Count != 0)
                {
                    foreach (var pizza in collection["selectedPizzas"])
                    {
                        var pizzaId = int.Parse(pizza);
                        var pizzaModel = this.pizzaService.Show(pizzaId);
                        pizzas.Add(new OrderPizza { Order = order, Pizza = pizzaModel });
                    }
                }

                order.Pizzas = pizzas;
                this.service.Update(id, order);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            var order = this.service.Show(id);
            var orderModel = new DeleteOrderViewModel { Id = order.Id, ClientName = order.ClientName, Address = order.Address };

            return View(orderModel);
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                this.service.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}