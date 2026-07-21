using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Task_Managment.BLL.DTOS.Task;

namespace Task_Managment.BLL.FluentValidation
{
    public class UpdateTaskValidator:AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator() {

            RuleFor(x => x.DueDate)
           .Must(x => x == null || x >= DateOnly.FromDateTime(DateTime.Today))
           .WithMessage("Due date must be today or in the future.");
        }
       
    }
}
