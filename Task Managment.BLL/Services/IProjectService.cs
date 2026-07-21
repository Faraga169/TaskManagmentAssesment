using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.BLL.Services
{
    public interface IProjectService
    {
        Task<PaginationDTO<ProjectDto>> GetProjectsAsync(ProjectSpecParams specParams); 
        Task<ProjectDetailsDto> GetByIdAsync(int id); 
        Task<ProjectDto> CreateAsync(CreateAndUpdateProjectDto dto); 
        Task<ProjectDto> UpdateAsync(int id, CreateAndUpdateProjectDto dto); 
        Task<int> DeleteAsync(int id);
    }
}
