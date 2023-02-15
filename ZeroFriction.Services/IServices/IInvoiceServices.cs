using ZeroFriction.Model.Models;
using ZeroFriction.Model.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFriction.Services.IServices
{
    public interface IInvoiceServices
    {
        Task<string> SaveInvoice(InvoiceDto dto);
        Task<string> UpdateInvoice(InvoiceDto dto);
    }
}
