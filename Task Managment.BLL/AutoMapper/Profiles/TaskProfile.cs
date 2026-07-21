using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Task_Managment.BLL.DTOS.Task;
using Task_Managment.DAL.Presisitence.Models;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.BLL.AutoMapper.Profiles
{
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskDTO>().ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name)); 
            CreateMap<Task, TaskDetailsDTO>().ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name)); 
            CreateMap<CreateTaskDto, Task>(); 
            CreateMap<UpdateTaskDto, Task>();
        }
    }
}
