using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Requests.Order
{
    public class UpdateOrderRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Phone] public string PhoneNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string Comments { get; set; }
    }
}
