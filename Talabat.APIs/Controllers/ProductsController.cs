using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Products;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : APIBaseController
	{
		private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductsController( // ASK CLR For creating object form IGenericRepository
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IGenericRepository<ProductCategory> categoryRepo,
			IMapper mapper
			) 
        {
			_productRepo = productRepo;
            _brandsRepo = brandRepo;
           _categoryRepo = categoryRepo;
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
				return NotFound(new ApiResponse(400));
			}
			var ProductToReturn = _mapper.Map<ProductToReturnDTO>(product);
			return Ok(ProductToReturn);
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable<ProductBrand>>> GetAllBrands()
		{
			var brands = await _brandsRepo.GetAllAsync();
			return Ok(brands);
		}
		[HttpGet("categories")]
		public async Task<ActionResult<IEnumerable<ProductCategory>>> GetAllCategories()
		{
			var categories = await _categoryRepo.GetAllAsync();
			return Ok(categories);
		}
    }
}
