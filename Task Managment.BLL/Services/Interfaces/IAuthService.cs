using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.DTOS.Auth;

namespace Task_Managment.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDTO dto); 
        Task<AuthResponseDto> LoginAsync(LoginDTO dto);
    }
}
