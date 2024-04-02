using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStockStatusRepository _stockStatusRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IProductRepository _productRepository;
        public WarehouseController(IMapper mapper, IPriceRepository priceRepository, IStockStatusRepository stockStatusRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _priceRepository = priceRepository;
            _stockStatusRepository = stockStatusRepository;
            _productRepository = productRepository;
        }

        [HttpGet("GetProductsWithStockStatus")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsWithStockStatus()
        {
            try
            {
                var productsList = await _stockStatusRepository.GetAllProductsWithStockStatusAsync();
                if (!productsList.Any())
                {
                    return BadRequest("Nie znaleziono produktów.");
                }

                var records = _mapper.Map<List<ProductDto>>(productsList);                

                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPrices")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetPrices()
        {
            try
            {
                var productList = await _productRepository.GetAllProductWithPrices();

                if (!productList.Any())
                {
                    return BadRequest("Nie znaleziono produktów.");
                }

                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(productList);
                
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreatePrice")]
        public async Task<IActionResult> CreatePrice(PriceDto dto)
        {
            try
            {
                await _priceRepository.AddAsync(_mapper.Map<Price>(dto));
                return Ok(new { success = true, message = "Cena została pomyślnie dodana!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice(PriceDto dto)
        {
            try
            {
                var previousPrice = await _priceRepository.GetAsync(dto.PriceId);
                _priceRepository.Detach(previousPrice);

                if (previousPrice.CompanyId == dto.CompanyId && previousPrice.Netto == dto.Netto)
                {
                    return Ok(new { success = true, message = "Cena została pomyślnie zaktualizowana!" });
                }
                else
                {
                    dto.PriceId = 0;
                    dto.CompanyId = previousPrice.CompanyId;
                    await _priceRepository.AddAsync(_mapper.Map<Price>(dto));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePrice{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            try
            {
                await _priceRepository.DeleteAsync(id);
                return Ok(new { success = true, message = "Cena została pomyślnie usunięta!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
