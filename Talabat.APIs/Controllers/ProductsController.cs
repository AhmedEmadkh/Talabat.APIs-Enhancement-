using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Products;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : APIBaseController
	{
		private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper) // ASK CLR For creating object form IGenericRepository
        {
			_productRepo = productRepo;
            _mapper = mapper;
        }
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetAllProducts()
		{
			var spec = new ProductWithIncludingBrandAndCategorySpec();
			var products = await _productRepo.GetAllWithSpecAsync(spec);
			var ProductsToReturn = _mapper.Map<IEnumerable<ProductToReturnDTO>>(products);
			return Ok(ProductsToReturn);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
		{
			var spec = new ProductWithIncludingBrandAndCategorySpec(id);
			var product = await _productRepo.GetByIdWithSpecAsync(spec);

			if(product is null)
			{
				return NotFound();
			}
			var ProductToReturn = _mapper.Map<ProductToReturnDTO>(product);
			return Ok(ProductToReturn);
		}
    }
}
