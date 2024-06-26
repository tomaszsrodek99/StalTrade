﻿using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        [HttpGet("GetPaymentMethods")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethods()
        {
            try
            {
                var methods = await _paymentMethodRepository.GetAllAsync();
                if (!methods.Any())
                {
                    return BadRequest("Nie znaleziono rekordów w bazie danych.");
                }
                return Ok(methods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("IsMethodExists")]
        public IActionResult IsMethodExists([FromBody] string request)
        {
            bool methodExists = _paymentMethodRepository.MethodExists(request);

            if (methodExists)
            {
                return BadRequest("Taka metoda płatności już istnieje.");
            }
            return Ok();
        }

        [HttpPost("CreatePaymentMethod")]
        public async Task<IActionResult> CreatePaymentMethod(PaymentMethod model)
        {
            try
            {
                await _paymentMethodRepository.AddAsync(model);
                return Ok(new { success = true, message = "Metoda płatności została pomyślnie dodana!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePaymentMethod{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            try
            {
                await _paymentMethodRepository.DeleteAsync(id);
                return Ok(new { success = true, message = "Pomyślnie usunięto metodę płatności!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
