using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Enums;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Managment.DAL.Presisitence.Models
{
    public class Task:BaseEntity
    {
        public int ProjectId { get; set; }

        /*Navigation Property*/
        public Project Project { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateOnly? DueDate { get; set; }
    }
}
