using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Services.Implementations;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SoccerFieldController : ControllerBase
    {
        private readonly ISoccerFieldService _soccerFieldService;

        private readonly IUserService _userService;

        private readonly IBookingService _bookingService;

        public SoccerFieldController(ISoccerFieldService soccerFieldService, IUserService userService, IBookingService bookingService)
        {
            _soccerFieldService = soccerFieldService;
            _userService= userService;
            _bookingService= bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSoccerFields()
        {
            try
            {
                var soccerFields = await _soccerFieldService.GetAllSoccerFieldsAsync();

                var soccerFieldResponses = soccerFields.Select(field => new SoccerFieldResponse
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

                return Ok(soccerFieldResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSoccerFieldById(int id)
        {
            try
            {
                var soccerField = await _soccerFieldService.GetSoccerFieldByIdAsync(id);

                if (soccerField == null)
                {
                    return NotFound(new { Message = "Soccer field not found." });
                }

                var soccerFieldResponse = new SoccerFieldResponse
                {
                    Id = soccerField.Id,
                    Name = soccerField.Name,
                    Description = soccerField.Description,
                    PictureUrl = soccerField.PictureUrl,
                    Price = soccerField.Price,
                    MinCapacity = soccerField.MinCapacity,
                    MaxCapacity = soccerField.MaxCapacity,
                    Indoor = soccerField.Indoor,
                    Owner = new UserResponse
                    {
                        Id = soccerField.Owner.Id,
                        Email = soccerField.Owner.Email,
                        Name = soccerField.Owner.Name,
                        PictureUrl = soccerField.Owner.PictureUrl,
                        RoleName = soccerField.Owner.Role?.Name
                    }
                };

                return Ok(soccerFieldResponse);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSoccerField([FromBody] SoccerFieldRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { Message = "Request body cannot be null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (request.OwnerId <= 0)
                {
                    return BadRequest(new { Message = "OwnerId is required and must be greater than 0." });
                }

                var owner = await _userService.GetUserByIdAsync(request.OwnerId);

                if (owner == null)
                {
                    return NotFound(new { Message = "Owner not found." });
                }

                var soccerField = new SoccerField
                {
                    Name = request.Name,
                    Description = request.Description,
                    PictureUrl = request.PictureUrl,
                    Price = request.Price,
                    MinCapacity = request.MinCapacity,
                    MaxCapacity = request.MaxCapacity,
                    Indoor = request.Indoor,
                    OwnerId = request.OwnerId,
                    Owner = owner
                };

                await _soccerFieldService.CreateSoccerFieldAsync(soccerField);

                var isAdded = await _userService.AddStadiumAsOwnerAsync(request.OwnerId, soccerField);

                if (!isAdded)
                {
                    return BadRequest(new { Message = "Failed to associate soccer field with owner." });
                }

                return CreatedAtAction(nameof(GetSoccerFieldById), new { id = soccerField.Id }, soccerField);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSoccerField(int id, [FromBody] SoccerFieldRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var soccerField = await _soccerFieldService.GetSoccerFieldByIdAsync(id);

                if (soccerField == null)
                {
                    return NotFound(new { Message = "Soccer field not found." });
                }

                var currentOwner = await _userService.GetUserByIdAsync(soccerField.OwnerId);

                if (currentOwner == null)
                {
                    return NotFound(new { Message = "Current owner not found." });
                }

                var newOwner = await _userService.GetUserByIdAsync(request.OwnerId);

                if (newOwner == null)
                {
                    return NotFound(new { Message = "New owner not found." });
                }

                soccerField.Name = request.Name;
                soccerField.Description = request.Description;
                soccerField.PictureUrl = request.PictureUrl;
                soccerField.Price = request.Price;
                soccerField.MinCapacity = request.MinCapacity;
                soccerField.MaxCapacity = request.MaxCapacity;
                soccerField.Indoor = request.Indoor;

                if (soccerField.OwnerId != request.OwnerId)
                {
                    currentOwner.OwnedFields.Remove(soccerField);

                    newOwner.OwnedFields.Add(soccerField);

                    soccerField.OwnerId = request.OwnerId;
                    soccerField.Owner = newOwner;
                }
                await _soccerFieldService.UpdateSoccerFieldAsync(soccerField.Id, soccerField);

                return Ok(soccerField);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoccerField(int id)
        {
            try
            {
                bool success = await _soccerFieldService.DeleteSoccerFieldAsync(id);
                return success ? NoContent() : NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{soccerFieldId}/bookings")]
        public async Task<IActionResult> GetBookingsBySoccerFieldId(int soccerFieldId)
        {
            try
            {
                var bookings = await _soccerFieldService.GetBookingsBySoccerFieldIdAsync(soccerFieldId);

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
