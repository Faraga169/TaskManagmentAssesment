using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.Specification;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.BLL.Specifications.ProjectSpecifications
{
    public class ProjectByIdSpecification:BaseSpecification<Project>
    {
        public ProjectByIdSpecification(int id, string ownerId) : base(p => p.Id == id && p.OwnerId == ownerId) 
        { 
            AddInclude(p => p.Tasks); 
        }
    }
}
