using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products_Spec
{
    public class ProductWithIncludingBrandAndCategorySpec : BaseSpecifications<Product>
    {
        // This Constructor will be Used for Creating an object, that will be used to get all Products
        public ProductWithIncludingBrandAndCategorySpec() : base()
        {
            AddIncludes();
        }
        // This Constructor will be used for creating an object, that will be used to get specific object
        public ProductWithIncludingBrandAndCategorySpec(int id):base(P => P.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
