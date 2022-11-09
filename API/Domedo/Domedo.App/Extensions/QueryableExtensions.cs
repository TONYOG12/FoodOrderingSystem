using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> CustomIgnoreQueryFilters<TEntity>(
            [NotNull] this IQueryable<TEntity> source, bool ignore = true)
            where TEntity : class
        {
            return ignore
                ? source.Provider is EntityQueryProvider
                    ? source.Provider.CreateQuery<TEntity>(
                        Expression.Call(
                            typeof(EntityFrameworkQueryableExtensions),
                            nameof(EntityFrameworkQueryableExtensions.IgnoreQueryFilters),
                            new[] { source.ElementType },
                            source.Expression))
                    : source
                : source;
        }
    }
}
