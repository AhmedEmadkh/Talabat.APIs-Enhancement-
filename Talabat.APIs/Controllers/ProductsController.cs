using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Products;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : APIBaseController
	{
        private readonly IProductService _productService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///      private readonly IGenericRepository<ProductBrand> _brandsRepo;
        ///      private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductsController( // ASK CLR For creating object form IGenericRepository
            IProductService productService,
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<ProductBrand> brandRepo,
            ///IGenericRepository<ProductCategory> categoryRepo,
			IMapper mapper
			) 
        {
            _productService = productService;
            ///_productRepo = productRepo;
            ///         _brandsRepo = brandRepo;
            ///        _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
		[Authorize]
		[HttpGet] // GET: /api/Products
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetAllProducts([FromQuery]ProductSpecParams specParams)
		{
			var products = await _productService.GetProductsAsync(specParams);

			var ProductsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDTO>>(products);

			var count = await _productService.GetCountAsync(specParams);

			var PaginatedProducts = new Pagination<ProductToReturnDTO>(specParams.PageIndex,specParams.PageSize, count, ProductsToReturn);
			
			return Ok(PaginatedProducts);
		}
		[HttpGet("{id}")] // GET: /api/Products/id
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);

			if(product is null)
			{
				return NotFound(new ApiResponse(400));
			}
			var ProductToReturn = _mapper.Map<ProductToReturnDTO>(product);
			return Ok(ProductToReturn);
		}

		[HttpGet("brands")] // GET: /api/Products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
		{
			var brands = await _productService.GetBrandsAsync();
			return Ok(brands);
		}
		[HttpGet("categories")] // GET: /api/Products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
		{
			var categories = await _productService.GetCategoriesAsync();
			return Ok(categories);
		}
    }
}
