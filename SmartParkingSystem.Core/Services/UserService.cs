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
        private readonly IEmailService _emailService;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, IJwtService jwtService, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
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

        public async Task<PaginationResponse<List<UserDTO>, object>> GetPagedUsersAsync(
            int page = 1,
            int pageSize = 10,
            string searchTerm = null)
        {
            try
            {
                var query = _userManager.Users.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();

                    query = query.Where(u =>
                        EF.Functions.Like(u.FullName.ToLower(), $"%{searchTerm}%") ||
                        EF.Functions.Like(u.Email.ToLower(), $"%{searchTerm}%"));
                }

                int totalUsers = await query.CountAsync();

                var users = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDTOs = new List<UserDTO>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault() ?? "Unknown";

                    var userDTO = _mapper.Map<UserDTO>(user);
                    userDTO.Role = role;

                    userDTOs.Add(userDTO);
                }

                return new PaginationResponse<List<UserDTO>, object>(
                    true,
                    "Users received successfully.",
                    payload: userDTOs,
                    pageNumber: page,
                    pageSize: pageSize,
                    totalCount: totalUsers
                );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<List<UserDTO>, object>(false, "Error: " + ex.Message);
            }
        }

        public async Task<ServiceResponse> ChangeUserRoleAsync(string userId, string newRole)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ServiceResponse(false, "User not found.");
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                var result = await _userManager.AddToRoleAsync(user, newRole);

                if (result.Succeeded)
                {
                    string emailBody = $"<h1>Ваша роль була змінена на: {newRole}.</h1><p>Якщо у вас є питання, будь ласка, дайте відповідь на цей лист.</p>";
                    await _emailService.SendEmailAsync(user.Email, "Зміна ролі користувача на Smart Parking System", emailBody);

                    return new ServiceResponse(true, "User role updated successfully.");
                }
                return new ServiceResponse(false, "Failed to update user role.", errors: result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, "Error: " + ex.Message);
            }
        }

        public async Task<ServiceResponse> ToggleBlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse(false, "User not found.");
            }

            user.LockoutEnd = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now
                ? (DateTimeOffset?)null
                : DateTimeOffset.UtcNow.AddYears(100);
            user.LockoutEnabled = user.LockoutEnd.HasValue;


            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ServiceResponse(true, user.LockoutEnd == null ? "User successfully unblocked." : "User successfully blocked.");
            }

            return new ServiceResponse(false, "Failed to toggle block status.", errors: result.Errors.Select(e => e.Description));
        }
    }
}
