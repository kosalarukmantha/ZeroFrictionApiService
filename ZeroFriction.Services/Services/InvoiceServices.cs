using ZeroFriction.Common.Validation;
using ZeroFriction.Model.Models;
using ZeroFriction.Model.Request;
using ZeroFriction.Repository.IRepository;
using ZeroFriction.Services.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFriction.Services.Services
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly ILogger<InvoiceServices> _logger;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceServices(IInvoiceRepository invoiceRepository, ILogger<InvoiceServices> logger)
        {
            _logger = logger;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<string> SaveInvoice(InvoiceDto dto)
        {
            try
            {
                dto.partitionKey = dto.InvoiceID + "Invo";
                //dto.Id = dto.InvoiceID.ToString();
                return await Task.FromResult(await _invoiceRepository.SaveInvoice(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown at InvoiceServices -> SaveInvoice : {0}", ex);
                throw;
            }
        }

        public async Task<string> UpdateInvoice(InvoiceDto dto)
        {
            try
            {
                dto.partitionKey = dto.InvoiceID + "Invo";
                //dto.Id = dto.InvoiceID.ToString();
                return await Task.FromResult(await _invoiceRepository.UpdateInvoice(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown at InvoiceServices -> UpdateInvoice : {0}", ex);
                throw;
            }
        }
    }
}
