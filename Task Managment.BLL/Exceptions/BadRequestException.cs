using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException(string message) : base(message) { 
        
        }
    }
}
