using ZeroFriction.Model.Models;
using ZeroFriction.Model.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFriction.Repository.IRepository
{
    public interface IInvoiceRepository
    {
        Task<string> SaveInvoice(InvoiceDto dto);
        Task<string> UpdateInvoice(InvoiceDto dto);
    }
}
