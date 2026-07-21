using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Task_Managment.BLL.DTOS.Project;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.AutoMapper.Profiles
{
    public class ProjectProfile:Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>(); 
            CreateMap<Project, ProjectDetailsDto>().ForMember(dest => dest.TasksCount, opt => opt.MapFrom(src => src.Tasks.Count)); 
            CreateMap<CreateAndUpdateProjectDto, Project>(); 
        }
    }
}
