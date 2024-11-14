using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.DTOs.Token
{
    public class Tokens
    {
        public string Token { get; set; } = string.Empty;
        public RefreshToken refreshToken { get; set; }
    }
}
