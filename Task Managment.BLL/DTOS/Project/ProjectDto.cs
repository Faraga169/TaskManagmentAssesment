using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.DTOS.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
