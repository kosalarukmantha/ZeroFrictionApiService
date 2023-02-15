using ZeroFriction.Model.Models;
using ZeroFriction.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroFriction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceServices _invoiceServices;

        public InvoiceController(IInvoiceServices invoiceServices, ILogger<InvoiceController> logger)
        {
            _logger = logger;
            _invoiceServices = invoiceServices;
        }

        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> SaveInvoice([FromBody] InvoiceDto dto)
        {
            var response = await _invoiceServices.SaveInvoice(dto);
            return Ok(response);
        }

        [HttpPost("EditInvoice")]
        public async Task<IActionResult> UpdateInvoice([FromBody] InvoiceDto dto)
        {
            var response = await _invoiceServices.UpdateInvoice(dto);
            return Ok(response);
        }

    }
}
