using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task_Managment.DAL.Enums;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Presisitence.Models;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Managment.DAL.Presisitence.Seeding
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TaskManagemntDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await context.Database.MigrateAsync();

            await SeedUsers(userManager);

            await SeedProjects(context, userManager);
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
                return;

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@test.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin@123");
        }

        private static async Task SeedProjects(TaskManagemntDbContext context,UserManager<ApplicationUser> userManager)
        {
            if (context.Projects.Any())
                return;

            var admin = await userManager.FindByEmailAsync("admin@test.com");

            if (admin is null)
                return;

            var project = new Project
            {
                Name = "Task Management API",
                Description = "Backend Assessment",
                OwnerId = admin.Id
            };

            context.Projects.Add(project);

            await context.SaveChangesAsync();

            context.Tasks.AddRange(

                new Models.Task
                {
                    ProjectId = project.Id,
                    Title = "Design Database",
                    Description = "Create entities",
                    Status = TaskStatus.Todo,
                    Priority = TaskPriority.High,
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
                },

                new Models.Task
                {
                    ProjectId = project.Id,
                    Title = "Implement JWT",
                    Description = "Authentication",
                    Status = TaskStatus.InProgress,
                    Priority = TaskPriority.Medium,
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
                },

                new Models.Task
                {
                    ProjectId = project.Id,
                    Title = "Write Unit Tests",
                    Description = "Testing",
                    Status = TaskStatus.Done,
                    Priority = TaskPriority.Low,
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(8))
                });

            await context.SaveChangesAsync();
        }

    }
}
