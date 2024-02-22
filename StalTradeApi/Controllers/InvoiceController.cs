using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using StalTradeAPI.Repositories;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStockStatusRepository _stockStatusRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceProductRepository _invoiceProductRepository;
        public InvoiceController(IMapper mapper, IProductRepository productRepository, IStockStatusRepository stockStatusRepository,
            ICompanyRepository companyRepository, IInvoiceRepository invoiceRepository, IInvoiceProductRepository invoiceProductRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _stockStatusRepository = stockStatusRepository;
            _companyRepository = companyRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceProductRepository = invoiceProductRepository;
        }

        [HttpGet("GetInvoices")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceRepository.GetAllInvoicesAsync();
                if (!invoices.Any())
                {
                    return BadRequest("Nie znaleziono faktur");
                }
                var records = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(InvoiceDto dto)
        {
            try
            {
                await _invoiceRepository.AddAsync(_mapper.Map<Invoice>(dto));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteInvoice{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                await _invoiceRepository.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
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
