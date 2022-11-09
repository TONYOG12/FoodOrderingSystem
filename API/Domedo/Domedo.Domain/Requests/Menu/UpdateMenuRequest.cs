using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Requests.Menu
{
    public class UpdateMenuRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
