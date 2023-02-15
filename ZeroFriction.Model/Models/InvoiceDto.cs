using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZeroFriction.Model.Models
{
    public class InvoiceDto
    {
        public Guid id { get; set; }
        public int InvoiceID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceLinesDto> InvoiceLinesDtos { get; set; }

        public string partitionKey { get; set; }
    }
}
