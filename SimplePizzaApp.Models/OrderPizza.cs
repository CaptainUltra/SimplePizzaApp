using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Models
{
    public class OrderPizza
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }
    }
}
