using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.BLL.Services;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAll([FromQuery] TaskSpecParams specParams) 
        {
            var tasks = await _taskService.GetTasksAsync(specParams);
            return Ok(tasks); 
        }
        
        [HttpGet("{id:int}")] 
        public async Task<IActionResult> Get(int id) 
        {
            var taskDetails = await _taskService.GetByIdAsync(id);
            return Ok(taskDetails); 
        }

        [HttpPut("{id:int}")] 
        public async Task<IActionResult> Update(int id, UpdateTaskDto dto) 
        {
            await _taskService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")] 
        public async Task<IActionResult> Delete(int id) 
        { 
            await _taskService.DeleteAsync(id); 
            return NoContent(); 
        }

       
    }
}
