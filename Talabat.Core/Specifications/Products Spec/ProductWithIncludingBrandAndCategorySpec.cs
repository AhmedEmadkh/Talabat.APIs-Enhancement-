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
        public ProductWithIncludingBrandAndCategorySpec(string sort, int? brandId, int? categoryId) : base(
            
            P => 

                    (!brandId.HasValue || P.BrandId == brandId.Value) &&
                    (!categoryId.HasValue || P.CategoryId == categoryId.Value)
            )
        {
            AddIncludes();


            // Adding the Order Expression
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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
