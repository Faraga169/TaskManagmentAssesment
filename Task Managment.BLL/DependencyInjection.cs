using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Managment.BLL.AutoMapper.Profiles;
using Task_Managment.BLL.Helper;
using Task_Managment.BLL.Services;

namespace Task_Managment.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services) 
        { 
            services.AddAutoMapper(o=>o.AddProfiles(new List<Profile> { new ProjectProfile(), new TaskProfile() })); 
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IJwtService, JwtService>(); services.AddScoped<IAuthService, AuthService>(); 
            services.AddScoped<ICurrentUserService, CurrentUserService>(); 
            services.AddHttpContextAccessor(); 
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            return services; 
        }
    }
}
