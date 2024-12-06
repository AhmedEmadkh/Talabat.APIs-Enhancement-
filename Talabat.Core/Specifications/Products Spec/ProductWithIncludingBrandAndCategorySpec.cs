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
        public ProductWithIncludingBrandAndCategorySpec(ProductSpecParams specParams) : base(
            
            P => 

                    (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                    (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        {
            AddIncludes();


            // Adding the Order Expression
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }


            // Product Number = 18 
            // PageSize = 5
            // PageIndex = 3

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize,specParams.PageSize);


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
