using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.Specification;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.BLL.Specifications.ProjectSpecifications
{
    public class ProjectsSpecification:BaseSpecification<Project>
    {
        public ProjectsSpecification(string ownerId, ProjectSpecParams specParams) : base(p => p.OwnerId == ownerId) 
        { 
           AddOrderByDescending(p => p.CreatedAt); 
           ApplyPaging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize); 
        }
    }
}
