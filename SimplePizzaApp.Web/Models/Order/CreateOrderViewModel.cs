using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePizzaApp.Web.Models.Order
{
    public class CreateOrderViewModel
    {
        public string ClientName { get; set; }
        public string Address { get; set; }
    }
}
