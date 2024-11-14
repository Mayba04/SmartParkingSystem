using SmartParkingSystem.Core.DTOs.Token;
using SmartParkingSystem.Core.Entities;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface IJwtService
    {
        Task Create(RefreshToken token);
        Task Delete(RefreshToken token);
        Task Update(RefreshToken token);
        Task<RefreshToken?> Get(string token);
        Task<IEnumerable<RefreshToken>> GetAll();
        Task<Tokens> GenerateJwtTokensAsync(AppUser user);
        Task<ServiceResponse> VerifyTokenAsync(TokenRequestDto tokenRequest);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);
    }
}
