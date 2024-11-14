using FluentValidation;
using SmartParkingSystem.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Validation.User
{
    public class LoginUserValidation : AbstractValidator<UserLoginDTO>
    {
        public LoginUserValidation()
        {
            RuleFor(r => r.Email).NotEmpty().WithMessage("Field must not be empty").EmailAddress().WithMessage("Invalid email format");
            RuleFor(r => r.Password).NotEmpty().WithMessage("Field must not be empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 charters")
                .MaximumLength(128);

        }
    }
}
