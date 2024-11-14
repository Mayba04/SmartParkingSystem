using SmartParkingSystem.Core.DTOs.User;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDTO, object>> AddAsync(UserCreateDTO model);
        Task<ServiceResponse<UserDTO, object>> UpdateAsync(UserUpdateDTO model);
        Task<ServiceResponse<object, object>> DeleteAsync(string id);
        Task<ServiceResponse<UserDTO, object>> GetByIdAsync(string id);
        Task<ServiceResponse<IEnumerable<UserDTO>, object>> GetAllAsync();

        Task<ServiceResponse> LoginUserAsync(UserLoginDTO model);
    }

}
