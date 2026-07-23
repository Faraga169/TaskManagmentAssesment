using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Specifications.Base;
using Task_Managment.DAL.Specifications.Parameters;

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
