using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
using Server.Models.Responses;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updatedUser);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);
                return success ? NoContent() : NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("{userId}/add-stadium")]
        public async Task<IActionResult> AddStadiumAsOwner(int userId, [FromBody] SoccerField soccerField)
        {
            try
            {
                await _userService.AddStadiumAsOwnerAsync(userId, soccerField);
                return Ok(new { Message = "Stadium added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{userId}/owned-fields")]
        public async Task<IActionResult> GetOwnedFields(int userId)
        {
            try
            {
                var fields = await _userService.GetOwnedFieldsAsync(userId);

                var response = fields.Select(field => new SoccerFieldResponse
                {
                    Id = field.Id,
                    Name = field.Name,
                    Description = field.Description,
                    PictureUrl = field.PictureUrl,
                    Price = field.Price,
                    MinCapacity = field.MinCapacity,
                    MaxCapacity = field.MaxCapacity,
                    Indoor = field.Indoor,
                    Owner = new UserResponse
                    {
                        Id = field.Owner.Id,
                        Email = field.Owner.Email,
                        Name = field.Owner.Name,
                        PictureUrl = field.Owner.PictureUrl,
                        RoleName = field.Owner.Role?.Name
                    }
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{userId}/owned-bookings")]
        public async Task<IActionResult> GetOwnedBookings(int userId)
        {
            try
            {
                var bookings = await _userService.GetOwnedBookingsAsync(userId);

                var response = bookings.Select(booking => new BookingResponse
                {
                    Id = booking.Id,
                    FieldId = booking.FieldId,
                    FieldName = booking.Field.Name,
                    Creator = new UserResponse
                    {
                        Id = booking.Field.Owner.Id,
                        Email = booking.Field.Owner.Email,
                        Name = booking.Field.Owner.Name,
                        PictureUrl = booking.Field.Owner.PictureUrl,
                        RoleName = booking.Field.Owner.Role?.Name
                    },
                    MaxParticipants = booking.MaxParticipants
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{userId}/requested-bookings")]
        public async Task<IActionResult> GetRequestedBookings(int userId)
        {
            try
            {
                var bookings = await _userService.GetRequestedBookingsAsync(userId);

                var response = bookings.Select(booking => new BookingResponse
                {
                    Id = booking.Id,
                    FieldId = booking.FieldId,
                    FieldName = booking.Field.Name,
                    Creator = new UserResponse
                    {
                        Id = booking.Field.Owner.Id,
                        Email = booking.Field.Owner.Email,
                        Name = booking.Field.Owner.Name,
                        PictureUrl = booking.Field.Owner.PictureUrl,
                        RoleName = booking.Field.Owner.Role?.Name
                    },
                    MaxParticipants = booking.MaxParticipants
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{userId}/approved-bookings")]
        public async Task<IActionResult> GetApprovedBookings(int userId)
        {
            try
            {
                var bookings = await _userService.GetApprovedBookingsAsync(userId);

                var response = bookings.Select(booking => new BookingResponse
                {
                    Id = booking.Id,
                    FieldId = booking.FieldId,
                    FieldName = booking.Field.Name,
                    Creator = new UserResponse
                    {
                        Id = booking.Field.Owner.Id,
                        Email = booking.Field.Owner.Email,
                        Name = booking.Field.Owner.Name,
                        PictureUrl = booking.Field.Owner.PictureUrl,
                        RoleName = booking.Field.Owner.Role?.Name
                    },
                    MaxParticipants = booking.MaxParticipants
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}