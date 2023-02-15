using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.Common.Validation
{
    public class CommonValidation
    {
        public bool ValidateValues(int value)
        {
           
            if (value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
