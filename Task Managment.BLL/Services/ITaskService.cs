using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.BLL.Services
{
    public interface ITaskService
    {
        Task<PaginationDTO<TaskDTO>> GetTasksAsync(TaskSpecParams specParams); 
        Task<TaskDetailsDTO> GetByIdAsync(int id); 
        Task<TaskDTO> CreateAsync(CreateTaskDto dto); 
        Task<TaskDTO> UpdateAsync(int id, UpdateTaskDto dto); 
        Task<int> DeleteAsync(int id);
    }
}
