using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T> 
    {
        // criteria yaani ahna nhebou naamlou quey w ndakhlou el navigation property w au mm temps nekhdmou bel generic 
        // donc nrmml fel product repository naamlou var products.where (x=> x.ProductTypeId == typeId).Include(x=> x.ProductType).ToListAsync;
        // Criteria hiya el where heki 
        Expression<Func<T,bool>> Criteria {get;}
        List<Expression<Func<T,object>>> Includes {get;}
    }
}