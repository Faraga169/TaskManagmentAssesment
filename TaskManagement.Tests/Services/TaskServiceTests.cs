using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.BLL.Exceptions;
using Task_Managment.BLL.FluentValidation;
using Task_Managment.BLL.Services;
using Task_Managment.BLL.Services.Interfaces;
using Task_Managment.DAL.Enums;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Repositories.Interfaces;
using Task_Managment.DAL.Specifications.Base;
using Xunit;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Task_Managment.DAL.Presisitence.Models.Task;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Management.Tests;

public class TaskServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private readonly Mock<IGenericRepository<Project>> _projectRepository = new();

    private readonly Mock<IGenericRepository<TaskEntity>> _taskRepository = new();

    private readonly Mock<IMapper> _mapper = new();

    private readonly Mock<ICurrentUserService> _currentUser = new();

    private readonly Mock<ILogger<TaskService>> _logger = new();

    private readonly TaskService _service;

    private readonly CreateTaskValidator _validator = new();

    public TaskServiceTests()
    {
        _currentUser.Setup(x => x.UserId)
            .Returns("user-1");

        _unitOfWork
            .Setup(x => x.Repository<Project>())
            .Returns(_projectRepository.Object);

        _unitOfWork
            .Setup(x => x.Repository<TaskEntity>())
            .Returns(_taskRepository.Object);

        _service = new TaskService(
            _unitOfWork.Object,
            _mapper.Object,
            _currentUser.Object,
            _logger.Object);
    }

    [Fact]
    public async Task CreateForProjectAsync_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Title = "Testing",
            Description = "Description"
        };

        _projectRepository
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync((Project?)null);

        // Act
        Func<Task> act = async () =>
            await _service.CreateAsync(1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Project not found.");
    }


    [Fact]
    public async Task CreateForProjectAsync_ShouldSetStatusToTodo()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Title = "Testing",
            Description = "Description"
        };

        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            OwnerId = "user-1"
        };

        var task = new TaskEntity();

        _projectRepository
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync(project);

        _mapper
            .Setup(x => x.Map<TaskEntity>(dto))
            .Returns(task);

        _mapper
            .Setup(x => x.Map<TaskDTO>(task))
            .Returns(new TaskDTO());

        _taskRepository
            .Setup(x => x.AddAsync(It.IsAny<TaskEntity>()))
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(x => x.CompleteAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(1, dto);

        // Assert
        task.ProjectId.Should().Be(1);
        task.Status.Should().Be(TaskStatus.Todo);

        _taskRepository.Verify(
            x => x.AddAsync(It.IsAny<TaskEntity>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.CompleteAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        _taskRepository
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<TaskEntity>>()))
            .ReturnsAsync((TaskEntity?)null);

        Func<System.Threading.Tasks.Task> act = async () =>
            await _service.GetByIdAsync(1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Task not found.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        _taskRepository
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<TaskEntity>>()))
            .ReturnsAsync((TaskEntity?)null);

        Func<System.Threading.Tasks.Task> act = async () =>
            await _service.DeleteAsync(1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Task not found.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldLogWarning_WhenStatusChangesFromDoneToTodo()
    {
        var task = new TaskEntity
        {
            Id = 1,
            Status = TaskStatus.Done
        };

        var dto = new UpdateTaskDto
        {
            Status = TaskStatus.Todo
        };

        _taskRepository
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<TaskEntity>>()))
            .ReturnsAsync(task);

        _mapper.Setup(x => x.Map(dto, task));

        _mapper.Setup(x => x.Map<TaskDTO>(task))
               .Returns(new TaskDTO());

        _unitOfWork
            .Setup(x => x.CompleteAsync())
            .ReturnsAsync(1);

        await _service.UpdateAsync(1, dto);

        _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }


  

    [Fact]
    public void CreateTask_Should_Have_Error_When_DueDate_Is_In_The_Past()
    {
        var dto = new CreateTaskDto
        {
            Title = "Testing",
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.DueDate)
              .WithErrorMessage("Due date must be today or in the future.");
    }

    [Fact]
    public void CreateTask_Should_Not_Have_Error_When_DueDate_Is_Today()
    {
        var dto = new CreateTaskDto
        {
            Title = "Testing",
            DueDate = DateOnly.FromDateTime(DateTime.Today)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }

    [Fact]
    public void CreateTask_Should_Not_Have_Error_When_DueDate_Is_In_The_Future()
    {
        var dto = new CreateTaskDto
        {
            Title = "Testing",
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }

}