using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Task_Managment.DAL.Presisitence.Models
{
    public class ApplicationUser:IdentityUser
    {

        /*Navigation Property*/
        public ICollection<Project> Projects { get; set; }= new HashSet<Project>();
    }
}
