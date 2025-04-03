namespace ECommerce.Domain.DTOs
{
    public class CartCreationResponseDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<int> InvalidProductIds { get; set; } = new List<int>(); // List of invalid product IDs
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>(); // List of successfully added items

       
    }
}
