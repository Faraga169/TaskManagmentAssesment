using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Task_Managment.BLL.DTOS.Auth;
using Task_Managment.BLL.Exceptions;
using Task_Managment.BLL.Services.Interfaces;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService) : IAuthService
    {
        public async Task<AuthResponseDto> RegisterAsync(RegisterDTO dto) 
        { 
            if (await userManager.FindByEmailAsync(dto.Email) is not null) 
                throw new BadRequestException("Email already exists."); 
            if (await userManager.FindByNameAsync(dto.UserName) is not null) 
                throw new BadRequestException("Username already exists."); 
            var user = new ApplicationUser { UserName = dto.UserName, Email = dto.Email }; 
            var result = await userManager.CreateAsync(user, dto.Password); 
            if (!result.Succeeded) 
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description))); 
            var token = await jwtService.GenerateTokenAsync(user); 
            return new AuthResponseDto { Token = token, Expiration = DateTime.UtcNow.AddMinutes(60) }; }
        public async Task<AuthResponseDto> LoginAsync(LoginDTO dto) 
        { 
            var user = await userManager.FindByEmailAsync(dto.Email); 
            if (user is null) throw new UnauthorizedException("Invalid email or password."); 
            var validPassword = await userManager.CheckPasswordAsync(user, dto.Password); 
            if (!validPassword) throw new UnauthorizedException("Invalid email or password."); 
            var token = await jwtService.GenerateTokenAsync(user); 
            return new AuthResponseDto { Token = token, Expiration = DateTime.UtcNow.AddMinutes(60) }; }
    }
}
