using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.BLL.Exceptions;
using Task_Managment.BLL.Services.Interfaces;
using Task_Managment.BLL.Specifications.ProjectSpecifications;
using Task_Managment.BLL.Specifications.TaskSpecifications;
using Task_Managment.DAL.Enums;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Repositories;
using Task_Managment.DAL.Repositories.Interfaces;
using Task_Managment.DAL.Specifications.Parameters;
using Task = Task_Managment.DAL.Presisitence.Models.Task;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Managment.BLL.Services
{
    public class TaskService(IUnitOfWork _unitOfWork,IMapper _mapper,ICurrentUserService currentUserService,ILogger<TaskService> logger):ITaskService
    {
        public async Task<PaginationDTO<TaskDTO>> GetTasksAsync(TaskSpecParams specParams)
        {
            var spec = new TasksSpecification(currentUserService.UserId!, specParams);
            var countSpec = new TasksSpecification(currentUserService.UserId!, specParams, applyPaging: false);

            var tasks = await _unitOfWork.Repository<Task>().ListAsync(spec);

            var count = await _unitOfWork.Repository<Task>().CountAsync(countSpec);

            return new PaginationDTO<TaskDTO>
            {
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize,
                Count = count,
                Data = _mapper.Map<IReadOnlyList<TaskDTO>>(tasks)
            };
        }
       
        public async Task<TaskDetailsDTO> GetByIdAsync(int id)
        {
            var spec = new TaskByIdSpecification(id,currentUserService.UserId!);

            var task = await _unitOfWork.Repository<Task>().GetEntityWithSpecAsync(spec);

            if (task is null)
                throw new NotFoundException("Task not found.");

            return _mapper.Map<TaskDetailsDTO>(task);
        }
         
        
        public async Task<TaskDTO> CreateAsync(int projectId, CreateTaskDto dto)
        {
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(new ProjectByIdSpecification(projectId, currentUserService.UserId!));

            if (project is null)
                throw new NotFoundException("Project not found.");

            var task = _mapper.Map<Task>(dto);

            task.ProjectId = projectId;
            task.Status = DAL.Enums.TaskStatus.Todo;

            await _unitOfWork.Repository<Task>().AddAsync(task);

            await _unitOfWork.CompleteAsync();

            task.Project = project;

            return _mapper.Map<TaskDTO>(task);
        }
     


          public async Task<TaskDTO> UpdateAsync(int id,UpdateTaskDto dto)
        {
            var task = await _unitOfWork.Repository<Task>().GetEntityWithSpecAsync(new TaskByIdSpecification(id,currentUserService.UserId!));

            if (task is null)
                throw new NotFoundException("Task not found.");

            if (task.Status == TaskStatus.Done && dto.Status == TaskStatus.Todo)
            {
                logger.LogWarning(
                    "Task {TaskId} status changed from Done to Todo.",
                    task.Id);
            }

            _mapper.Map(dto, task);

           
            _unitOfWork.Repository<Task>().Update(task);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TaskDTO>(task);
        }
    


           public async Task<int> DeleteAsync(int id)
        {
            var task = await _unitOfWork.Repository<Task>().GetEntityWithSpecAsync(new TaskByIdSpecification(id, currentUserService.UserId!));

            if (task is null)
                throw new NotFoundException("Task not found.");

            _unitOfWork.Repository<Task>().Delete(task);

            return await _unitOfWork.CompleteAsync();
        }

     

        public async Task<PaginationDTO<TaskDTO>> GetProjectTasksAsync(int projectId,TaskSpecParams specParams)
        {
            var spec = new ProjectTasksSpecification(projectId,currentUserService.UserId!,specParams);

            var tasks = await _unitOfWork.Repository<Task>().ListAsync(spec);

            var count = await _unitOfWork.Repository<Task>().CountAsync(new ProjectTasksSpecification(projectId,currentUserService.UserId!,specParams,false));

            return new PaginationDTO<TaskDTO>
            {
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize,
                Count = count,
                Data = _mapper.Map<IReadOnlyList<TaskDTO>>(tasks)
            };
        }
    }
}
