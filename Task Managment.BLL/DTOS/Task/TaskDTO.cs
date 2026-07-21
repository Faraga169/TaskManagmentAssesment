using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Enums;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Managment.BLL.DTOS.Task
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public TaskStatus Status { get; set; }

        public TaskPriority Priority { get; set; }

        public DateOnly? DueDate { get; set; }

        public string ProjectName { get; set; } = null!;
    }
}
