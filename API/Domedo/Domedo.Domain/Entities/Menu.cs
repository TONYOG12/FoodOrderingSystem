using Domedo.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } 

    }
}
