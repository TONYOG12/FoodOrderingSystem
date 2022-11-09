using Domedo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Extensions
{
    public partial class OrderStats
    {
        public OrderStats()
        {
            TotalPrice = 0;
        }

        public decimal TotalPrice { get; set; } 

        public OrderStats Accumulate(OrderItem orderItem)
        {
            var price = orderItem.Meal.Price * orderItem.Quantity;

            TotalPrice += price;

            return this;
        }
    }
}
