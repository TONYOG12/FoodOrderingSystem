using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Utils
{
    public class RoleUtils
    {
        public const string AppRoleAdmin = "admin";
        public const string AppRoleSuper = "super";


        public static IEnumerable<string> AppRoles()
        {
            return new[]
            {
                AppRoleAdmin,
                AppRoleSuper
            };
        }
    }
}
