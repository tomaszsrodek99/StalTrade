using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using StalTradeAPI.Repositories;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IStockStatusRepository _stockStatusRepository;
        private readonly StalTradeDbContext _context;
        public ProductController(IMapper mapper, IProductRepository productRepository, IStockStatusRepository stockStatusRepository, StalTradeDbContext context)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _stockStatusRepository = stockStatusRepository;
            _context = context;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();

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

        [HttpGet("GetProduct{productId}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("IsProductUnique{productId}")]
        public IActionResult IsProductUnique(int productId, [FromBody] string companyDrawingNumber)
        {
            bool isProductUnique = _productRepository.IsProductExists(companyDrawingNumber, productId);
            if (isProductUnique)
            {
                return BadRequest("Taki produkt już istnieje.");
            }
            return Ok();
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var product = _mapper.Map<Product>(dto);
                await _productRepository.AddAsync(product);

                var stockStatus = new StockStatus
                {
                    ProductId = product.ProductId,
                    Product = product,
                    PurchasedQuantity = 0,
                    ActualQuantity = 0,
                    SoldQuantity = 0,
                    InStock = 0,
                    PurchasedValue = 0,
                    SoldValue = 0,
                    MarginValue = 0,
                    Margin = 0,
                };

                await _stockStatusRepository.AddAsync(stockStatus);

                product.StockStatusId = stockStatus.StockStatusId;

                await _productRepository.UpdateAsync(product);

                transaction.Commit();

                return Ok(new { success = true, message = "Produkt został pomyślnie dodany!" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            try
            {
                await _productRepository.UpdateAsync(_mapper.Map<Product>(dto));
                return Ok(new { success = true, message = "Produkt został pomyślnie edytowany!" });
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
                return Ok(new { success = true, message = "Produkt został pomyślnie usunięty!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
