using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Core.DTOs.User;
using SmartParkingSystem.Core.Entities;
using SmartParkingSystem.Core.Interfaces;
using SmartParkingSystem.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserDTO, object>> AddAsync(UserCreateDTO model)
        {
            var appUserEntity = _mapper.Map<UserCreateDTO, AppUser>(model);
            await _userManager.CreateAsync(appUserEntity);
            return new ServiceResponse<UserDTO, object>(true, "User created successfully", payload: _mapper.Map<UserDTO>(appUserEntity));
        }

        public async Task<ServiceResponse<UserDTO, object>> UpdateAsync(UserUpdateDTO model)
        {
            var appUserEntity = _mapper.Map<UserUpdateDTO, AppUser>(model);
            await _userManager.UpdateAsync(appUserEntity);
            return new ServiceResponse<UserDTO, object>(true, "User updated successfully", payload: _mapper.Map<UserDTO>(appUserEntity));
        }

        public async Task<ServiceResponse<object, object>> DeleteAsync(string id)
        {
            AppUser userdelete = await _userManager.FindByIdAsync(id);
            if (userdelete == null)
            {
                return new ServiceResponse(false, "User a was found");
            }
            await _userManager.DeleteAsync(userdelete);
            return new ServiceResponse<object, object>(true, "User deleted successfully");
        }

        public async Task<ServiceResponse<UserDTO, object>> GetByIdAsync(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return new ServiceResponse<UserDTO, object>(false, "User not found");
            }
            return new ServiceResponse<UserDTO, object>(true, "", payload: _mapper.Map<UserDTO>(appUser));
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>, object>> GetAllAsync()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserDTO> mappedUsers = users.Select(u => _mapper.Map<AppUser, UserDTO>(u)).ToList();

            return new ServiceResponse<IEnumerable<UserDTO>, object>(true, "", payload: _mapper.Map<IEnumerable<UserDTO>>(users));
        }
    }
}
