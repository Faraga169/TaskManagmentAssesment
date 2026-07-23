
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task = System.Threading.Tasks.Task; 
using Task_Managment.BLL;
using Task_Managment.BLL.Helper;
using Task_Managment.DAL;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Presisitence.Seeding;
using Task_Managment.PL.Extensions;
using System.Text.Json.Serialization;
namespace Task_Managment.PL
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

           

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

         
            builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddBusinessLayer(builder.Configuration);
            
            builder.Services.AddAuthorization();
           
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
          

            var app = builder.Build();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                await SeedData.InitializeAsync(app.Services);
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
           
            }
            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
       
            app.Run();

       
        
    }

      
    }

    
    
}
