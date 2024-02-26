using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Context;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IStockStatusRepository _stockStatusRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly StalTradeDbContext _context;
        public InvoiceController(IMapper mapper, IProductRepository productRepository, IStockStatusRepository stockStatusRepository,
            IInvoiceRepository invoiceRepository, StalTradeDbContext context)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _stockStatusRepository = stockStatusRepository;
            _invoiceRepository = invoiceRepository;
            _context = context;
        }

        [HttpGet("GetInvoices")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceRepository.GetAllInvoicesAsync();
                if (!invoices.Any())
                {
                    return BadRequest("Nie znaleziono zapisanych faktur.");
                }
                var records = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductsWithLatestPrices")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsWithLatestPrices()
        {
            try
            {
                var productList = await _productRepository.GetAllProductWithPrices();

                if (!productList.Any())
                {
                    return BadRequest("Nie znaleziono zapisanych produktów.");
                }
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(productList);

                foreach (var product in productDtos)
                {
                    var latestPurchasePrices = product.Prices
                        .Where(price => price.IsPurchase)
                        .GroupBy(price => new { price.ProductId, price.CompanyId })
                        .Select(group => group.OrderByDescending(price => price.Date).FirstOrDefault());

                    var latestSalePrices = product.Prices
                        .Where(price => !price.IsPurchase)
                        .GroupBy(price => new { price.ProductId, price.CompanyId })
                        .Select(group => group.OrderByDescending(price => price.Date).FirstOrDefault());

                    var latestPrices = new List<PriceDto>();

                    if (latestPurchasePrices != null)
                        latestPrices.AddRange(latestPurchasePrices);

                    if (latestSalePrices != null)
                        latestPrices.AddRange(latestSalePrices);

                    product.Prices = latestPrices;
                }

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(InvoiceDto dto)
        {
            using var transaction = _context.Database.BeginTransaction(); 

            try
            {
                var addedInvoice = await _invoiceRepository.AddAsync(_mapper.Map<Invoice>(dto));

                dto.InvoiceId = addedInvoice.InvoiceId;

                foreach (var product in dto.ProductsList)
                {
                    product.InvoiceId = dto.InvoiceId;
                    product.IsPurchase = dto.IsPurchase;
                    var stockStatus = await _stockStatusRepository.GetAsyncByProductId(product.ProductId);
                    
                    if (stockStatus == null)
                    {
                        transaction.Rollback();
                        return BadRequest($"Nie udało się znaleźć StockStatus dla produktu o ID {product.ProductId}.");
                    }

                    if (dto.IsPurchase == false)
                    {
                        stockStatus.SoldQuantity += product.Quantity;
                        stockStatus.InStock -= product.ActualQuantity;
                        stockStatus.SoldValue += product.Brutto;
                    }
                    else
                    {
                        stockStatus.PurchasedQuantity += product.Quantity;
                        stockStatus.InStock += product.ActualQuantity;
                        stockStatus.PurchasedValue += product.Brutto;
                        stockStatus.ActualQuantity += product.ActualQuantity;
                    }

                    await _stockStatusRepository.UpdateAsync(stockStatus);
                }

                transaction.Commit(); 

                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback(); 
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteInvoice{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var invoice = await _invoiceRepository.GetInvoiceWithProducts(id);

                await _invoiceRepository.DeleteAsync(id);

                foreach (var product in invoice.ProductsList)
                {
                    var stockStatus = await _stockStatusRepository.GetAsyncByProductId(product.ProductId);

                    if (stockStatus == null)
                    {
                        transaction.Rollback();
                        return BadRequest($"Nie udało się znaleźć StockStatus dla produktu o ID {product.ProductId}.");
                    }

                    if (invoice.IsPurchase == false)
                    {
                        stockStatus.SoldQuantity -= product.Quantity;
                        stockStatus.InStock += product.Quantity;
                        stockStatus.SoldValue -= product.Brutto;
                    }
                    else
                    {
                        stockStatus.PurchasedQuantity -= product.Quantity;
                        stockStatus.InStock -= product.ActualQuantity;
                        stockStatus.PurchasedValue -= product.Brutto;
                        stockStatus.ActualQuantity -= product.ActualQuantity;
                    }

                    await _stockStatusRepository.UpdateAsync(stockStatus);
                }

                transaction.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetInvoice{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            try
            {
                var invoice = await _invoiceRepository.GetAsync(id);
                var record = _mapper.Map<InvoiceDto>(invoice);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
