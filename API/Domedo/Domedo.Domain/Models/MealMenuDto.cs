using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Models
{
    public class MealMenuDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MealDto> Meals { get; set; }
    }
}
