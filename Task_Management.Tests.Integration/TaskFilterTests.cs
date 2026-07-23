using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.DAL.Enums;
using Xunit;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Management.IntegrationTests;

public class TaskFilterTests : IntegrationTestBase
{
    public TaskFilterTests(ApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetTasks_FilterByStatusAndPriority_ShouldReturnCorrectTasks()
    {
        await RegisterAndLoginAsync();

        // Create Project

        var projectResponse = await Client.PostAsJsonAsync(
            "/api/Project",
            new CreateAndUpdateProjectDto
            {
                Name = "Filter Project",
                Description = "Integration Test"
            });

        projectResponse.EnsureSuccessStatusCode();

        var project =
            await projectResponse.Content.ReadFromJsonAsync<ProjectDto>(JsonOptions);

        // Create High Priority Task

        await Client.PostAsJsonAsync(
            $"/api/Project/{project!.Id}/tasks",
            new CreateTaskDto
            {
                Title = "High Task",
                Description = "Task 1",
                Priority = TaskPriority.Low,
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3))
            });

        // Create Low Priority Task

        await Client.PostAsJsonAsync(
            $"/api/Project/{project.Id}/tasks",
            new CreateTaskDto
            {
                Title = "Low Task",
                Description = "Task 2",
                Priority = TaskPriority.Low,
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3))
            });

        // Filter

        var response = await Client.GetAsync(
            "/api/task?status=Todo&priority=Low&pageIndex=1&pageSize=5");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result =
            await response.Content.ReadFromJsonAsync<PaginationDTO<TaskDTO>>(JsonOptions);

        result.Should().NotBeNull();

        result!.Data.Should().OnlyContain(x =>
            x.Status == TaskStatus.Todo &&
            x.Priority == TaskPriority.Low);
    }
}