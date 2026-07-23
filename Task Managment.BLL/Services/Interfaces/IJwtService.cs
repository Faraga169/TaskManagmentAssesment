using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
