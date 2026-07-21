using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
