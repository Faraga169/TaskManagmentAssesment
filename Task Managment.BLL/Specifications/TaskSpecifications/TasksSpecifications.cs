using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Presisitence.Models;
using Task_Managment.DAL.Specifications.Base;
using Task_Managment.DAL.Specifications.Parameters;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.BLL.Specifications.TaskSpecifications
{
    public class TasksSpecification : BaseSpecification<Task>
    {
        public TasksSpecification(string ownerId, TaskSpecParams specParams, bool applyPaging = true) : base(t => t.Project.OwnerId == ownerId && 
        (string.IsNullOrEmpty(specParams.Search) || t.Title.ToLower().Contains(specParams.Search.ToLower())) && 
        (!specParams.Status.HasValue || t.Status == specParams.Status) && 
        (!specParams.Priority.HasValue || t.Priority == specParams.Priority) &&
        (!specParams.DueDateFrom.HasValue || t.DueDate >= specParams.DueDateFrom) && 
        (!specParams.DueDateTo.HasValue || t.DueDate <= specParams.DueDateTo)) 
        { 
            AddInclude(t => t.Project); 
            switch (specParams.Sort?.ToLower()) 
            { case "duedate": 
                    AddOrderBy(t => t.DueDate!); 
                    break; 
                case "priority": 
                    AddOrderByDescending(t => t.Priority); 
                    break; 
                case "title": 
                    AddOrderBy(t => t.Title); 
                    break; 
                default: 
                    AddOrderByDescending(t => t.CreatedAt); 
                    break; 
            }
            if (applyPaging)
            {
                ApplyPaging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
            }
        }
    }
}
