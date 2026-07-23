using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Specifications.Base;

namespace Task_Managment.BLL.Specifications.ProjectSpecifications
{
    public class ProjectByNameSpecification:BaseSpecification<Project>
    {
        public ProjectByNameSpecification(string ownerId, string projectName): base(p =>
          p.OwnerId == ownerId &&
          p.Name.ToLower() == projectName.ToLower())
        {
        }
    }
}
