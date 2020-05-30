using SimplePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePizzaApp.Web.Models.Order
{
    public class ShowOrderViewModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public List<OrderPizza> Pizzas { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
