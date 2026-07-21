using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Task_Managment.DAL.Enums;
using TaskStatus = Task_Managment.DAL.Enums.TaskStatus;

namespace Task_Managment.BLL.DTOS.Task
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,'()]+$", ErrorMessage = "Title contains invalid characters.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [EnumDataType(typeof(TaskPriority), ErrorMessage = "Invalid priority level selected.")]
        public TaskPriority Priority { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(TaskStatus), ErrorMessage = "Invalid status selected.")]
        public TaskStatus Status { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DueDate { get; set; }
    }
}
