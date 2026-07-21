using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.Exceptions
{
    public class UnauthorizedException:Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
