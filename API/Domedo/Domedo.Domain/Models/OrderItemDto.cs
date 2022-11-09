using Domedo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Models
{
    public class OrderItemDto
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
        //public Guid OrderId { get; set; }
    }
}
