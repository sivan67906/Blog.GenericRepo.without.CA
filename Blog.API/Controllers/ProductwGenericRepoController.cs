using Blog.API.DTOs;
using Blog.API.Models;
using Blog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductwGenericRepoController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;

        public ProductwGenericRepoController(IRepository<Product> productRepository
            , IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        //[HttpGet("{search}")]
        //public async Task<ActionResult<IEnumerable<Product>>> Search(string name)
        //{
        //    var product = await _productRepository.Search(name);

        //    if (product is null)
        //        return NotFound();

        //    return Ok(product);
        //}

        [HttpGet("Product Name")]
        public async Task<IActionResult> GetName(string productName)
        {
            var result = await _productRepository.GetProductByName(productName);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO productRequest)
        {
            var entity = new Product()
            {
                ProductName = productRequest.ProductName,
                Price = productRequest.Price
            };

            var createProduct = await _productRepository.AddAsync(entity);
            var orderEntity = new Order()
            {
                OrderDate = DateTime.Now,
                ProductId = 1
                //ProductId = createProduct.ProductId
            };
            await _orderRepository.AddAsync(orderEntity);

            return CreatedAtAction(nameof(GetById), new { id = createProduct.ProductId }, createProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO product)
        {
            var entity = await _productRepository.GetByIdAsync(id);

            if (entity is null)
                return NotFound();

            entity.ProductName = product.ProductName;
            entity.Price = product.Price;

            await _productRepository.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);

            if (entity is null)
                return NotFound();

            await _productRepository.DeleteAsync(entity);

            return NoContent();
        }
    }
}
