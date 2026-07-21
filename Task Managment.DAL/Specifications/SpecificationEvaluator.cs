using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_Managment.BLL.Specification;

namespace Task_Managment.DAL.Specifications
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecification<T> specification)
        {
            var query = inputQuery;

            // Where
            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes
            query = specification.Includes.Aggregate(
                query,
                (current, include) => current.Include(include));

            // Order By
            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            // Order By Desc
            if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Pagination
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }

    }
}
