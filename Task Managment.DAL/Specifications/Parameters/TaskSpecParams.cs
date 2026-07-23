using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Enums;

namespace Task_Managment.DAL.Specifications.Parameters
{
    public class TaskSpecParams
    {
        private const int MaxPageSize = 20;

        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Search { get; set; }

        public Enums.TaskStatus? Status { get; set; }

        public TaskPriority? Priority { get; set; }

        public DateOnly? DueDateFrom { get; set; }

        public DateOnly? DueDateTo { get; set; }

        public string? Sort { get; set; }
    }
}
