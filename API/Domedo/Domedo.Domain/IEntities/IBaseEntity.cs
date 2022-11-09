using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.IEntities
{
    public interface IBaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? LastDeletedBy { get; set; }
    }
}
