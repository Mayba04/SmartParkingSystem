using FluentValidation;
using SmartParkingSystem.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Validation.User
{
    public class ChangeUserRoleValidation : AbstractValidator<ChangeUserRoleDTO>
    {
        public ChangeUserRoleValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.NewRole)
                .NotEmpty().WithMessage("New role is required.")
                .Must(BeAValidRole).WithMessage("Role is invalid.");
        }

        private bool BeAValidRole(string role)
        {
            var validRoles = new List<string> { "User", "Seller", "Administrator", "ModeratorSeller", "Administrator2" };
            return validRoles.Contains(role);
        }
    }
}
