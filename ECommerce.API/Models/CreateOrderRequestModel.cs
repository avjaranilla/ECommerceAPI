namespace ECommerce.API.Models
{
    public class CreateOrderRequestModel
    {
        public int CartId { get; set; }  // CartID is provided
        public string ShippingAddress { get; set; } // Shipping Address
    }
}
