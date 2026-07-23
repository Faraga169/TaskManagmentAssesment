using System.Net;
using System.Net.Http.Json;
using Azure;
using FluentAssertions;
using Microsoft.OpenApi;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.DAL.Enums;
using Xunit;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Management.IntegrationTests;

public class ProjectFlowTests : IntegrationTestBase
{
    public ProjectFlowTests(ApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateProject_AddTask_MarkDone_DeleteProject_ShouldWork()
    {
        await RegisterAndLoginAsync();

        // Create Project

        var projectDto = new CreateAndUpdateProjectDto
        {
            Name = "Integration Project",
            Description = "Testing"
        };

        var projectResponse =
            await Client.PostAsJsonAsync("/api/project", projectDto);

        projectResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var project =
            await projectResponse.Content.ReadFromJsonAsync<ProjectDto>(JsonOptions);

        // Create Task

        var createTask = new CreateTaskDto
        {
            Title = "Task 1",
            Description = "Integration",
            Priority = TaskPriority.High,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
        };

        var taskResponse =
            await Client.PostAsJsonAsync(
                $"/api/project/{project!.Id}/tasks",
                createTask);

        taskResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var task =
            await taskResponse.Content.ReadFromJsonAsync<TaskDTO>(JsonOptions);

        // Update

        var update = new UpdateTaskDto
        {
            Title = "Task 1",
            Description = "Integration",
            Priority = TaskPriority.High,
            Status = TaskStatus.Done,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
        };

        var updateResponse =
            await Client.PutAsJsonAsync(
                $"/api/task/{task!.Id}",
                update);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Delete Project

        var deleteResponse =
            await Client.DeleteAsync($"/api/project/{project.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify Cascade Delete

        var getTask =
            await Client.GetAsync($"/api/task/{task.Id}");

        getTask.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}