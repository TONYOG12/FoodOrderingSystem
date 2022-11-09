using Domedo.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Phone] public string PhoneNumber { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string Comments { get; set; }    
        public decimal TotalPrice { get; set; }
        public bool Completed { get; set; } = false;
    }
}
