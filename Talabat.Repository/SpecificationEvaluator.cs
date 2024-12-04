using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> spec)
        {
            var query = inputQuery; // _dbContext.Set<T>()

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria); // _dbContext.Set<T>().Where(P => P.Id == 1)
            }

            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
            // query = _dbContext.Set<T>().Where(P => P.Id == 1).Include(P => P.Brand)
            // query = _dbContext.Set<T>().Where(P => P.Id == 1).Include(P => P.Brand).Include(P => P.Category)
            return query;
        }
    }
}
