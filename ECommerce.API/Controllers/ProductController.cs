using ECommerce.API.Models;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        const string notFound = "Product not found.";

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await  _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = notFound });

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequestModel model)
        {
            var newProduct = new Product(model.Name, model.Description, model.Price, model.StockQuantity);

            var product = await _productService.CreateProductAsync(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product model)
        {
            var product = await _productService.GetProductByIdAsync(model.Id);
            if (product == null)
                return NotFound(new { message = notFound });

            product.Update(model.Name, model.Description, model.Price, model.StockQuantity, model.IsActive);
            await _productService.UpdateProductAsync(product);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (result == null)
                return NotFound(new { message = notFound });
            else
                await _productService.DeleteProductAsync(id);
                return NoContent(); // 204 No Content if successfully deleted
        }
    }
}
