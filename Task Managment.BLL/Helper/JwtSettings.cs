using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.Helper
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings"; 
        public string Key { get; set; } = string.Empty; 
        public string Issuer { get; set; } = string.Empty; 
        public string Audience { get; set; } = string.Empty; 
        public int DurationInMinutes { get; set; }
    }
}
