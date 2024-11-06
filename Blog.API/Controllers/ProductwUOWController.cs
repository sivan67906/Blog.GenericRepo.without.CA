using Blog.API.DTOs;
using Blog.API.Models;
using Blog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductwUOWController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductwUOWController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _unitOfWork.GetRepository<Product>().GetAllAsync();

        return Ok(result);
    }

    [HttpGet("Product Name")]
    public async Task<IActionResult> GetName(string productName)
    {
        var result = await _unitOfWork.ProductService.GetProductByName(productName);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDTO product)
    {
        try
        {
            using var transaction = _unitOfWork.BeginTransactionAsync();

            var entity = new Product()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };

            var pResult = await _unitOfWork.GetRepository<Product>().AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            var orderEntity = new Order()
            {
                OrderDate = DateTime.Now,
                ProductId = 1
                //ProductId = pResult.ProductId
            };

            var oResult = await _unitOfWork.GetRepository<Order>().AddAsync(orderEntity);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return StatusCode((int)HttpStatusCode.Created, new { id = pResult.ProductId });
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}