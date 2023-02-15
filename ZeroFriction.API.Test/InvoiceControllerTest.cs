using ZeroFriction.API.Controllers;
using ZeroFriction.Model.Models;
using ZeroFriction.Services.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.API.Test
{
    public class InvoiceControllerTest
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly InvoiceServices _invoiceServices;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SaveInvoiceTest_DataSave_ReturnTrue()
        {
            //Arrage
            var _invoiceController = new InvoiceController(_invoiceServices, _logger);

            //Act
            var result = _invoiceController.SaveInvoice(new InvoiceDto { Description="kosala", TotalAmount=5, InvoiceID= 120});
            //Assert
            Assert.IsTrue(result.IsCompleted);
        }
        //Need to write the other Unit test
    }
}
