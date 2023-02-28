using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        // el methode hedhy taamel fi nafs el haja hedhy 
        //.Include(p=>p.ProductBrand)
        //.Include(p=> p.ProductType)
        // TEntoty hiya par exp Product
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spec)
        {
            // inputQuery houwa el tableau
            var query = inputQuery;

            if(spec.Criteria != null)
            {
                //get me a product where the product is whatever we've specified as this criteria
                query = query.Where(spec.Criteria); // p => p.ProductTypeId == id
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}