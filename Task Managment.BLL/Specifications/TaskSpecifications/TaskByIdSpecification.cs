using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.Specification;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.Specifications.TaskSpecifications
{
    public class TaskByIdSpecification:BaseSpecification<DAL.Presisitence.Models.Task>
    {
        public TaskByIdSpecification(int id, string ownerId) : base(t => t.Id == id && t.Project.OwnerId == ownerId) 
        { 
            AddInclude(t => t.Project); 
        }
    }
}
