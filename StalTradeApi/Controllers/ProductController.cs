using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductController(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductWithPricesAsync();
                if (!products.Any())
                {
                    return BadRequest("Nie znaleziono produktów");
                }
                var records = _mapper.Map<List<ProductDto>>(products);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("IsProductUnique/{productId}", Name = "IsProductUnique")]
        public IActionResult IsProductUnique(int productId, string companyDrawingNumber)
        {
            return new JsonResult(_productRepository.IsProductExists(companyDrawingNumber, productId));
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            try
            {
                await _productRepository.AddAsync(_mapper.Map<Product>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            try
            {
                await _productRepository.UpdateAsync(_mapper.Map<Product>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProduct{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
