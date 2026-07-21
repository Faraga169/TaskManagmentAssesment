using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task_Managment.BLL.DTOS.Project
{
    public class CreateAndUpdateProjectDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,'()]+$",
             ErrorMessage = "Name can only contain letters, numbers, spaces, and basic punctuation.")]
        public string Name { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }
    }
}
