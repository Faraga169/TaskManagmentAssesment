using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Task_Managment.BLL.DTOS.Common;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.BLL.Exceptions;
using Task_Managment.BLL.Services.Interfaces;
using Task_Managment.BLL.Specifications.ProjectSpecifications;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Repositories;
using Task_Managment.DAL.Repositories.Interfaces;
using Task_Managment.DAL.Specifications.Parameters;

namespace Task_Managment.BLL.Services
{
    public class ProjectService(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserService currentUserService) : IProjectService
    {
        public async Task<PaginationDTO<ProjectDto>> GetProjectsAsync(ProjectSpecParams specParams)
        {
            var spec = new ProjectsSpecification(currentUserService.UserId, specParams);
            var projects = await _unitOfWork.Repository<Project>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Project>().CountAsync(spec);
            return new PaginationDTO<ProjectDto> { PageIndex = specParams.PageIndex, PageSize = specParams.PageSize, Count = count, Data = _mapper.Map<IReadOnlyList<ProjectDto>>(projects) };
        }

        public async Task<ProjectDetailsDto> GetByIdAsync(int id)
        {
            var spec = new ProjectByIdSpecification(id, currentUserService.UserId);
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(spec);
            if (project is null)
                throw new NotFoundException("Project not found.");
            return _mapper.Map<ProjectDetailsDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateAndUpdateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.OwnerId = currentUserService.UserId!;
            var existingProject = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(new ProjectByNameSpecification(currentUserService.UserId!, dto.Name));


            if (existingProject is not null)
                throw new BadRequestException("A project with the same name already exists.");
            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProjectDto>(project);
        }


        public async Task<ProjectDto> UpdateAsync(int id, CreateAndUpdateProjectDto dto)
        {
            var spec = new ProjectByIdSpecification(id, currentUserService.UserId);
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(spec);
            if (project is null) 
                throw new NotFoundException("Project not found.");
            _mapper.Map(dto, project);
            var existingProject = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(new ProjectByNameSpecification(currentUserService.UserId!, dto.Name));
            if (existingProject is not null && existingProject.Id != id)
                throw new BadRequestException("A project with the same name already exists.");
            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProjectDto>(project);

        }

        public async Task<int> DeleteAsync(int id) 
        { 
            var spec = new ProjectByIdSpecification(id, currentUserService.UserId); 
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpecAsync(spec); 
            if (project is null) 
                throw new NotFoundException("Project not found."); 
            _unitOfWork.Repository<Project>().Delete(project); 
            return await _unitOfWork.CompleteAsync(); }
    }
}
