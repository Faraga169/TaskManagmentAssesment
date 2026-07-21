using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Repositories;
using Task_Managment.DAL.Repositories.Interfaces;

namespace Task_Managment.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<TaskManagemntDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            return services;
        }

        
    }
}
