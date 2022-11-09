using Domedo.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid MealId { get; set; }
        public Meal Meal { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}
