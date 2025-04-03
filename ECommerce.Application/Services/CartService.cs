using ECommerce.Application.Interfaces;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUserCartRepository _userCartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IUserCartRepository userCartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository)
        {
            _userCartRepository = userCartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<UserCartDTO> UpsertItemsInTheCartAsync(int cartId, List<ProductOrderModel> productDetails)
        {
            // Step 1: Validate input
            if (productDetails == null || productDetails.Count == 0)
            {
                throw new ArgumentException("Cart must have at least one valid item.");
            }

            // Step 2: Fetch existing cart and its items
            var cart = await _userCartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
            {
                throw new ArgumentException("Cart not found.");
            }

            var existingItems = await _cartItemRepository.GetItemsByCartIdsAsync(new List<int> { cartId });

            // Step 3: Fetch product details for validation
            var productIds = productDetails.Select(p => p.ProductId).ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            var validProducts = new List<CartItem>();
            var invalidItems = new List<int>();

            // Step 4: Process new items (validate and upsert)
            foreach (var productDetail in productDetails)
            {
                var product = products.FirstOrDefault(p => p.Id == productDetail.ProductId);

                if (product == null || productDetail.Quantity > product.StockQuantity)
                {
                    invalidItems.Add(productDetail.ProductId);
                    continue;
                }

                var existingItem = existingItems.FirstOrDefault(ci => ci.ProductId == productDetail.ProductId);

                if (existingItem != null)
                {
                    // Update existing item quantity and price
                    existingItem.Quantity = productDetail.Quantity;
                    existingItem.UnitPrice = product.Price;
                    existingItem.UpdateTotal();
                    await _cartItemRepository.UpdateAsync(existingItem);
                }
                else
                {
                    // Add new item
                    var newItem = new CartItem
                    {
                        CartId = cartId,
                        ProductId = productDetail.ProductId,
                        Quantity = productDetail.Quantity,
                        UnitPrice = product.Price
                    };

                    newItem.UpdateTotal();
                    await _cartItemRepository.AddAsync(newItem);
                }
            }

            // Step 5: Identify and remove items no longer in the request
            var newProductIds = productDetails.Select(p => p.ProductId).ToHashSet();
            var itemsToRemove = existingItems.Where(ci => !newProductIds.Contains(ci.ProductId)).ToList();

            foreach (var item in itemsToRemove)
            {
                await _cartItemRepository.RemoveAsync(item.CartItemId);
            }

            // Step 6: Return updated cart DTO
            var updatedCart = await _userCartRepository.GetCartByIdAsync(cartId);
            return new UserCartDTO
            {
                CartId = updatedCart.CartId,
                UserId = updatedCart.UserId,
                Status = updatedCart.Status,
                CartItems = updatedCart.CartItems.Select(i => new CartItemDTO
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalAmount = i.TotalAmount
                }).ToList()
            };
        }

        public async Task<UserCartDTO> GetCartByIdAsync(int cartId)
        {
            // Step 1: Retrieve the cart
            var cart = await _userCartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
            }

            // Step 2: Retrieve items for this cart
            var cartItems = await _cartItemRepository.GetItemsByCartIdsAsync(new List<int> { cartId });

            var productIds = cartItems.Select(ci => ci.ProductId).Distinct().ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // Step 3: Map to DTO
            var cartDto = new UserCartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Status = cart.Status,
                DateCreated = cart.DateCreated,
                TotalAmount = cartItems
                    .Where(ci => ci.CartId == cart.CartId)
                    .Sum(ci => ci.TotalAmount), // Calculate total amount
                CartItems = cartItems
                    .Where(ci => ci.CartId == cart.CartId)
                    .Select(ci =>
                    {
                        var product = products.FirstOrDefault(p => p.Id == ci.ProductId);
                        return new CartItemDTO
                        {
                            ProductId = ci.ProductId,
                            ProductName = product?.Name ?? "Unknown", // Handle missing product name
                            Quantity = ci.Quantity,
                            UnitPrice = ci.UnitPrice,
                            TotalAmount = ci.TotalAmount
                        };
                    })
                    .ToList()
            };

            return cartDto;
        }

        public async Task<IEnumerable<UserCartDTO>> GetCartByUserIdAsync(int userId)
        {
            // Step 1: Fetch all carts for the given user
            var userCarts = await _userCartRepository.GetCartByUserIdAsync(userId);

            if (userCarts == null)
            {
                return new List<UserCartDTO>(); // Return empty list if no carts found
            }

            // Step 2: Get all cart items for the retrieved carts
            var cartIds = userCarts.Select(c => c.CartId).ToList();
            var cartItems = await _cartItemRepository.GetItemsByCartIdsAsync(cartIds);

            // Step 3: Get product details for the cart items
            var productIds = cartItems.Select(ci => ci.ProductId).Distinct().ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // Step 4: Map to DTOs
            var cartDtos = userCarts.Select(cart => new UserCartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Status = cart.Status,
                DateCreated = cart.DateCreated,
                TotalAmount = cartItems
                    .Where(ci => ci.CartId == cart.CartId)
                    .Sum(ci => ci.TotalAmount), // Calculate total amount

                CartItems = cartItems
                    .Where(ci => ci.CartId == cart.CartId)
                    .Select(ci =>
                    {
                        var product = products.FirstOrDefault(p => p.Id == ci.ProductId);
                        return new CartItemDTO
                        {
                            ProductId = ci.ProductId,
                            ProductName = product?.Name ?? "Unknown", // Handle missing product name
                            Quantity = ci.Quantity,
                            UnitPrice = ci.UnitPrice,
                            TotalAmount = ci.TotalAmount
                        };
                    })
                    .ToList()

            }).ToList();

            return cartDtos;
        }

        public async Task<CartCreationResponseDTO> CreateCartAsync(int userId, List<ProductOrderModel> productDetails)
        {
            // Step 1: Validate prod details to ensure at least 1 item is provided.
            if (productDetails == null || productDetails.Count == 0)
            {
                throw new ArgumentException("Cart must have at least one item.");
            }

            // Step 2: Fetch prod details from the prod repository for validation.
            var productIds = productDetails.Select(p => p.ProductId).ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // Step 3: Filter valid product details and prepare cart items
            var validProductDetails = new List<ProductOrderModel>();
            var invalidItems = new List<int>(); // To store invalid item IDs for later logging or reporting

            foreach (var productDetail in productDetails)
            {
                var product = products.FirstOrDefault(p => p.Id == productDetail.ProductId);

                // Skip invalid products
                if (product == null)
                {
                    invalidItems.Add(productDetail.ProductId); // Add to invalid list
                    continue; // Skip this product and move to the next
                }

                // Validate stock availability
                if (productDetail.Quantity > product.StockQuantity)
                {
                    invalidItems.Add(productDetail.ProductId); // Add to invalid list
                    continue; // Skip this product and move to the next
                }

                // If valid, add to the valid list
                validProductDetails.Add(productDetail);
            }

            // Step 4: Proceed if we have valid items to process.
            if (validProductDetails.Count == 0)
            {
                throw new ArgumentException("No valid products to add to the cart.");
            }

            // Step 5: Create a new cart (UserCart).
            var userCart = await _userCartRepository.AddAsync(userId);

            // Step 6: Create CartItems for each valid product and add them to the cart.
            var cartItems = new List<CartItemDTO>();
            decimal totalAmount = 0;

            foreach (var product in validProductDetails)
            {
                var productDetail = products.First(p => p.Id == product.ProductId);

                var cartItem = new CartItem
                {
                    CartId = userCart.CartId,
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    UnitPrice = productDetail.Price,
                };

                cartItem.UpdateTotal(); // Calculate total price per item
                totalAmount += cartItem.TotalAmount;

                await _cartItemRepository.AddAsync(cartItem); // Save CartItem to database

                // Add cart item to DTO list
                cartItems.Add(new CartItemDTO
                {
                    ProductId = cartItem.ProductId,
                    ProductName = products.First(p => p.Id == product.ProductId).Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TotalAmount = cartItem.TotalAmount
                });
            }

            // Step 7: Return the DTO with the created cart and invalid items.
            return new CartCreationResponseDTO
            {
                CartId = userCart.CartId,
                UserId = userId,
                TotalAmount = totalAmount,
                InvalidProductIds = invalidItems,
                CartItems = cartItems
            };
        }
    }
}
