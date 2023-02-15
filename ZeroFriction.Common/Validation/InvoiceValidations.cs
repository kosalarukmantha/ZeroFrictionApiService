using ZeroFriction.Model.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.Common.Validation
{
    public class InvoiceValidations
    {
        public InvoiceRequest ValidateSumOfMaxCapacityForGroup(int sumMaxCapacity, int capacity, int sumMaxCapacityForStation)
        {
            var response = new InvoiceRequest();
            // Group should be there
            if (capacity > 0)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Group Id that not belongs to any group. Please check the Group Id and retry.";
                return response;
            }

            if (capacity >= sumMaxCapacity + sumMaxCapacityForStation)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Connectors max capacity exceed the value of capacity for the group.";
            }
            return response;
        }
        public InvoiceRequest ValidateGroup( int capacity)
        {
            var response = new InvoiceRequest();
            // Group should be there
            if (capacity > 0)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Group Id that not belongs to any group. Please check the Group Id and retry.";
                return response;
            }

            return response;
        }

    }
}
