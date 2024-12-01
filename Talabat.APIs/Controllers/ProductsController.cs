using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : APIBaseController
	{
		private readonly IGenericRepository<Product> _productRepo;

		public ProductsController(IGenericRepository<Product> productRepo) // ASK CLR For creating object form IGenericRepository
		{
			_productRepo = productRepo;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
		{
			var spec = new ProductWithIncludingBrandAndCategorySpec();
			var products = await _productRepo.GetAllWithSpecAsync(spec);
			return Ok(products);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var spec = new ProductWithIncludingBrandAndCategorySpec(id);
			var product = await _productRepo.GetByIdWithSpecAsync(spec);

			if(product is null)
			{
				return NotFound();
			}
			return Ok(product);
		}
    }
}
