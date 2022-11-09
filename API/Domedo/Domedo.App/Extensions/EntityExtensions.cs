using Domedo.Domain.IEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsDeleted(this IBaseEntity item)
        {
            return item.DeletedAt != null;
        }

    }
}
