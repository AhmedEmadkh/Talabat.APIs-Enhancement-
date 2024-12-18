using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;

        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork


            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo
            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;


            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket For Basket Repo

            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItem>();
            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered,product.Price,item.Quantity);

                    orderItems.Add(orderItem);
                }
            }
            // 3. Calculate Subtotal

            var subTotal = orderItems.Sum(orderIteme => orderIteme.Price * orderIteme.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order

            var Order = new Order(buyerEmail,shippingAddress,deliveryMethod,orderItems,subTotal);

            await _unitOfWork.Repository<Order>().AddAsync(Order);

            // 6. Save To Database

            var result = await _unitOfWork.CompleteAsync();

            if(result <= 0)
                return null;
            return Order;
        }

        public Task<Order> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
        public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
