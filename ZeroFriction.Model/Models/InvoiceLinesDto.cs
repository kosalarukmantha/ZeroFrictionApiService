using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.Model.Models
{
    public class InvoiceLinesDto
    {
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }

    }
}
