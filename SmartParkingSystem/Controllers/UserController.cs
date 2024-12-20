﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Core.DTOs.User;
using SmartParkingSystem.Core.Interfaces;
using SmartParkingSystem.Core.Responses;
using SmartParkingSystem.Core.Services;
using SmartParkingSystem.Core.Validation.User;

namespace SmartParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _appUserService;

        public UserController(IUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO model)
        {
            var validationResult = await new LoginUserValidation().ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse response = await _appUserService.LoginUserAsync(model);
                return Ok(response);
            }
            return BadRequest(validationResult.Errors.FirstOrDefault());
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _appUserService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _appUserService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO model)
        {
            var response = await _appUserService.AddAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO model)
        {
            var response = await _appUserService.UpdateAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _appUserService.DeleteAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUserss(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string searchTerm = null)
        {
            var response = await _appUserService.GetPagedUsersAsync(pageNumber, pageSize, searchTerm);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("changerole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDTO model)
        {
            var validationResult = await new ChangeUserRoleValidation().ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var response = await _appUserService.ChangeUserRoleAsync(model.UserId, model.NewRole);
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            return BadRequest(validationResult.Errors.FirstOrDefault());
        }

        [HttpPost("toggleblock")]
        public async Task<IActionResult> ToggleBlockUser([FromBody] string UserId)
        {
            var response = await _appUserService.ToggleBlockUserAsync(UserId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
