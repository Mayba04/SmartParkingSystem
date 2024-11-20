using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.DTOs.User
{
    public class ChangeUserRoleDTO
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}
