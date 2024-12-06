using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products_Spec
{
    public class ProductWithFilterationForCountSpec : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpec(ProductSpecParams specParams)
            :base(P => 
            
                    (!specParams.BrandId.HasValue || specParams.BrandId.Value == P.BrandId) &&
                    (!specParams.CategoryId.HasValue || specParams.CategoryId.Value == P.CategoryId)
            )
        {
            
        }
    }
}
