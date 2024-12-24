using AutoMapper;
using Talabat.APIs.DTOs.Orders;
using Talabat.APIs.DTOs.Products;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureURLResolver>());

            CreateMap<AddressDTO, Address>();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethodName, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.ProductUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.ProductUrl, O => O.MapFrom<OrderItemPictureURLRespolver>());

        }
    }
}
