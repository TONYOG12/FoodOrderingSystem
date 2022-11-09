using Domedo.Domain.IEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Entities
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        [PersonalData] public string FirstName { get; set; }
        [PersonalData] public string LastName { get; set; }
        //public string UserType { get; set; }
        //public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? LastDeletedBy { get; set; }
    }
}
