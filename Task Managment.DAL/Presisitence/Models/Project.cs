using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.DAL.Presisitence.Models
{
    public class Project:BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string OwnerId { get; set; } = null!;

        /*Navigation property*/
        public ApplicationUser Owner { get; set; } = null!;

        public ICollection<Task> Tasks { get; set; }= new HashSet<Task>();
    }
}
