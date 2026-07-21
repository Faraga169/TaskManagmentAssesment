using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Task_Managment.BLL.Helper;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.Services
{
    public class JwtService(IOptions<JwtSettings> options) : IJwtService
    {
        private readonly JwtSettings _settings = options.Value;

        public async Task<string> GenerateTokenAsync(ApplicationUser user) 
        { 
              var claims = new List<Claim> { 
                new(JwtRegisteredClaimNames.Sub, user.Id), 
                new(JwtRegisteredClaimNames.Email, user.Email!), 
                new(ClaimTypes.NameIdentifier, user.Id), 
                new(ClaimTypes.Name, user.UserName!) }; 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)); 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
            var token = new JwtSecurityToken(issuer: _settings.Issuer, audience: _settings.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes), signingCredentials: credentials); 
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
