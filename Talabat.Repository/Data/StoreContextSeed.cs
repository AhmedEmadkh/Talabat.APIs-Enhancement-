using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public static async Task SeedAsync(StoreContext _dbContext)
		{
			#region Seeding Brands
			if (!_dbContext.ProductBrands.Any())
			{
				var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands?.Count() > 0)
				{
					foreach (var brand in brands)
					{
						await _dbContext.Set<ProductBrand>().AddAsync(brand);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			#endregion
			#region Seeding Categories
			if (!_dbContext.ProductCategories.Any())
			{
				var categoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);

				if (categories?.Count() > 0)
				{
					foreach (var category in categories)
					{
						await _dbContext.Set<ProductCategory>().AddAsync(category);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			#endregion
			#region Seeding Products
			if (!_dbContext.Products.Any())
			{

				var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);

				if (products?.Count() > 0)
				{
					foreach (var product in products)
					{
						await _dbContext.Set<Product>().AddAsync(product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
            #endregion
            #region Seeding Delivery Methods
            if (!_dbContext.DeliveryMethod.Any())
			{
				var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
				if(deliveryMethodsData?.Count() > 0)
				{
					foreach(var deliveryMethod in deliveryMethods)
					{
						await _dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);
					}
					await _dbContext.SaveChangesAsync();
				}
            }
			#endregion
		}
	}
}
