using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePizzaApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderPizza> Pizzas { get; set; }
    }
}
