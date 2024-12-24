using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);

        public Task<Product?> GetProductByIdAsync(int productId);

        public Task<int> GetCountAsync(ProductSpecParams specParams);

        public Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
    }
}
