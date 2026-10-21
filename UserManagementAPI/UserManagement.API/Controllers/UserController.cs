using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = await _service.CreateAsync(dto);

            return Created(
                $"/api/users/{user.Id}",
                user
            );
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateUser(Guid id, UpdateUserDto model)
        {
            var user = await _service.UpdateUserAsync(id, model);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
