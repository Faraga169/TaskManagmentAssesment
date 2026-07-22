using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.BLL.Services;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController(IProjectService _projectService,ITaskService _taskService) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAll([FromQuery] ProjectSpecParams specParams) 
        {
            var projects = await _projectService.GetProjectsAsync(specParams);
            return Ok(projects); 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return Ok(project);
        }

       
        [HttpPost] 
        public async Task<IActionResult> Create(CreateAndUpdateProjectDto dto) 
            { 
                var createProject = await _projectService.CreateAsync(dto); 
                return CreatedAtAction(nameof(Get), new { id = createProject.Id }, createProject); 
            }

        [HttpPut("{id:int}")] 
        public async Task<IActionResult> Update(int id, CreateAndUpdateProjectDto dto) 
        {
            await _projectService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id:int}")] 
        public async Task<IActionResult> Delete(int id) 
        { 
            await _projectService.DeleteAsync(id); 
            return NoContent(); 
        
        }


        [HttpGet("{id:int}/tasks")]
        public async Task<IActionResult> GetProjectTasks(int id,[FromQuery] TaskSpecParams specParams)
        {
            var projectTasks = await _taskService.GetProjectTasksAsync(id, specParams);
            return Ok(projectTasks);
        }

        [HttpPost("{id:int}/tasks")]
        public async Task<IActionResult> CreateTask(int id,CreateTaskDto dto)
        {
            var task = await _taskService.CreateForProjectAsync(id, dto);

            return CreatedAtAction(nameof(TaskController.Get),"Task",new { id = task.Id },task);
        }

    }
}
