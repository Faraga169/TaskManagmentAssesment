
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task_Managment.BLL;
using Task_Managment.BLL.Helper;
using Task_Managment.DAL;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Presisitence.Seeding;
using Task_Managment.PL.Extensions;
namespace Task_Managment.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddBusinessLayer();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options => {
                    var jwt = builder.Configuration.GetSection(JwtSettings.SectionName)
                    .Get<JwtSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = jwt!.Issuer, ValidAudience = jwt.Audience, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)) };
                });
            builder.Services.AddAuthorization();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            await SeedData.InitializeAsync(app.Services);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }
            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirection();
            
         
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
       
            app.Run();
        }
    }
}
