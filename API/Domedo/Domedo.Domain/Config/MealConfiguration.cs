using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domedo.Domain.Entities;

namespace Domedo.Domain.Config
{
    public class MealConfigurtion : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.HasQueryFilter(item => item.DeletedAt == null);

        }
    }
}
