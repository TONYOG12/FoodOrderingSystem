using Domedo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Models
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Phone] public string PhoneNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comments { get; set; }
        public bool Completed { get; set; }

    }
}
