using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs.Orders
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethodName { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
        public decimal Subtotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
