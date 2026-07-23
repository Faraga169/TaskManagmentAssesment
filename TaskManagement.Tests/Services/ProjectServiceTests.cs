using AutoMapper;
using FluentAssertions;
using Moq;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.Exceptions;
using Task_Managment.BLL.Services;
using Task_Managment.BLL.Services.Interfaces;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Repositories.Interfaces;
using Task_Managment.DAL.Specifications.Base;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Task_Management.Tests.Unit.Services;

public class ProjectServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IGenericRepository<Project>> _projectRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();

    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _currentUserMock.Setup(x => x.UserId)
                        .Returns("user-1");

        _unitOfWorkMock
            .Setup(x => x.Repository<Project>())
            .Returns(_projectRepositoryMock.Object);

        _projectService = new ProjectService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserMock.Object);
    }
    
    
    [Fact]
    public async Task CreateAsync_ShouldThrowBadRequestException_WhenProjectNameAlreadyExists()
    {
        // Arrange
        var dto = new CreateAndUpdateProjectDto
        {
            Name = "Task Management",
            Description = "Test Project"
        };

        _mapperMock
            .Setup(x => x.Map<Project>(dto))
            .Returns(new Project());

        _projectRepositoryMock
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync(new Project
            {
                Id = 1,
                Name = dto.Name,
                OwnerId = "user-1"
            });

        // Act
        Func<Task> act = async () =>
            await _projectService.CreateAsync(dto);

        // Assert
        await act.Should()
                 .ThrowAsync<BadRequestException>()
                 .WithMessage("A project with the same name already exists.");
    }


    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync((Project?)null);

        // Act
        Func<Task> act = async () =>
            await _projectService.GetByIdAsync(1);

        // Assert
        await act.Should()
                 .ThrowAsync<NotFoundException>()
                 .WithMessage("Project not found.");
    }


    [Fact]
    public async Task DeleteAsync_ShouldThrowNoسtFoundException_WhenProjectDoesNotExist()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync((Project?)null);

        // Act
        Func<Task> act = async () =>
            await _projectService.DeleteAsync(1);

        // Assert
        await act.Should()
                 .ThrowAsync<NotFoundException>()
                 .WithMessage("Project not found.");
    }


    [Fact]
    public async Task CreateAsync_ShouldCreateProjectSuccessfully()
    {
        // Arrange
        var dto = new CreateAndUpdateProjectDto
        {
            Name = "Project 1",
            Description = "Description"
        };

        var project = new Project
        {
            Name = dto.Name
        };

        _mapperMock
            .Setup(x => x.Map<Project>(dto))
            .Returns(project);

        _mapperMock
            .Setup(x => x.Map<ProjectDto>(project))
            .Returns(new ProjectDto
            {
                Id = 1,
                Name = project.Name
            });

        _projectRepositoryMock
            .Setup(x => x.GetEntityWithSpecAsync(It.IsAny<ISpecification<Project>>()))
            .ReturnsAsync((Project?)null);

        _projectRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Project>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CompleteAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _projectService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();

        _projectRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Project>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.CompleteAsync(),
            Times.Once);
    }
}
