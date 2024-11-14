using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Core.DTOs.Token;
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
        private readonly IJwtService _jwtService;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse> LoginUserAsync(UserLoginDTO model)
        {
            AppUser? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse(false, "User or password incorect.");
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password,false, lockoutOnFailure: true);
            if (result.IsNotAllowed)
            {
                return new ServiceResponse(false, "Confirm your email please");
            }
            if (result.IsLockedOut)
            {
                return new ServiceResponse(false, "User is locked. Connect with your site admistrator.");
            }
            if (result.Succeeded)
            {
                Tokens? tokens = await _jwtService.GenerateJwtTokensAsync(user);
                await _signInManager.SignInAsync(user, false);
                return new ServiceResponse(true, "User successfully loged in.", payload: true, accessToken: tokens.Token, refreshToken: tokens.refreshToken.Token);
            }
            return new ServiceResponse(false, "User or password incorect");
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
