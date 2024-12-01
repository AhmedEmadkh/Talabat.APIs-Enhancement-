using AutoMapper;
using Talabat.APIs.DTOs.Products;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseURL"]}/{source.PictureUrl}";
            return string.Empty;
        }
    }
}
