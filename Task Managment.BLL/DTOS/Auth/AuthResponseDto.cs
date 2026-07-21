using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.DTOS.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }
}
