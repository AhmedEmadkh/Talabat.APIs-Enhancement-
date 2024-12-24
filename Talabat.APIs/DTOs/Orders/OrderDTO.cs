using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.Orders
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Delivery Method Is Required")]
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
