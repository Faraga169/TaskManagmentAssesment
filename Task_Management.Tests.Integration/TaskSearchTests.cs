using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.DAL.Enums;
using Xunit;

namespace Task_Management.IntegrationTests;

public class TaskSearchTests : IntegrationTestBase
{
    public TaskSearchTests(ApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task SearchTasks_WithPagination_ShouldReturnExpectedResults()
    {
        // Arrange
        await RegisterAndLoginAsync();

        var projectResponse = await Client.PostAsJsonAsync(
            "/api/Project",
            new CreateAndUpdateProjectDto
            {
                Name = "Search Project",
                Description = "Integration Test"
            });

        projectResponse.EnsureSuccessStatusCode();

        var project =
            await projectResponse.Content.ReadFromJsonAsync<ProjectDto>(JsonOptions);

        // Create 10 Tasks
        for (int i = 1; i <= 10; i++)
        {
            await Client.PostAsJsonAsync(
                $"/api/Project/{project!.Id}/tasks",
                new CreateTaskDto
                {
                    Title = i <= 6
                        ? $"API Task {i}"
                        : $"Frontend Task {i}",

                    Description = "Testing",

                    Priority = TaskPriority.Medium,

                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
                });
        }

        // Act
        var response = await Client.GetAsync(
            "/api/task?search=API&pageIndex=1&pageSize=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result =
            await response.Content.ReadFromJsonAsync<PaginationDTO<TaskDTO>>(JsonOptions);

        result.Should().NotBeNull();

        result!.PageIndex.Should().Be(1);

        result.PageSize.Should().Be(5);

        result.Count.Should().Be(6);

      
        result.Data.Should().HaveCount(5);

        result.Data.Should()
            .OnlyContain(x => x.Title.Contains("API"));
    }
}