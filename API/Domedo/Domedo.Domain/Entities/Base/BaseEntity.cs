using Domedo.Domain.IEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? LastDeletedBy { get; set; }
    }
}
