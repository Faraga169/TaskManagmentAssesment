using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.Exceptions
{
    public class ForbiddenException:Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
