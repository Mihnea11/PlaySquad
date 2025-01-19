using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        private readonly IUserService _userService;
        private readonly ISoccerFieldService _soccerFieldService;

        public BookingController(IBookingService bookingService, IUserService userService, ISoccerFieldService soccerFieldService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _soccerFieldService = soccerFieldService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();

                var bookingResponses = bookings.Select(booking => new BookingResponse
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

                return Ok(bookingResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                {
                    return NotFound(new { Message = "Booking not found." });
                }

                var bookingResponse = new BookingResponse
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
                };

                return Ok(bookingResponse);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creator = await _userService.GetUserByIdAsync(bookingRequest.CreatorId);

                if (creator == null)
                {
                    return NotFound(new { Message = "Creator not found." });
                }

                var field=await _soccerFieldService.GetSoccerFieldByIdAsync(bookingRequest.FieldId);

                if (field == null)
                {
                    return NotFound(new { Message = "Field not found." });
                }
                var booking = new Booking
                {
                    FieldId = bookingRequest.FieldId,
                    CreatorId = bookingRequest.CreatorId,
                    Creator=creator,
                    Field=field,
                    MaxParticipants = bookingRequest.MaxParticipants,
                    WaitingList = new List<User>(),
                    ApprovedParticipants = new List<User>()
                };

                var createdBooking = await _bookingService.CreateBookingAsync(booking);
                creator.OwnedBookings.Add(booking);
                field.Bookings.Add(booking);

                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);

                if (existingBooking == null)
                {
                    return NotFound(new { Message = "Booking not found." });
                }

                existingBooking.FieldId = bookingRequest.FieldId;
                existingBooking.CreatorId = bookingRequest.CreatorId;
                existingBooking.MaxParticipants = bookingRequest.MaxParticipants;

                _bookingService.UpdateBookingAsync(existingBooking.Id, existingBooking);

                return Ok(existingBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                bool success=await _bookingService.DeleteBookingAsync(id);
                return success ? NoContent() : NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{bookingId}/add-to-waiting-list")]
        public async Task<IActionResult> AddUserToWaitingList(int bookingId, [FromBody] int userId)
        {
            try
            {
                await _bookingService.AddUserToWaitingListAsync(bookingId, userId);
                return Ok(new { Message = "User added to waiting list successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{bookingId}/remove-from-waiting-list/{userId}")]
        public async Task<IActionResult> RemoveUserFromWaitingList(int bookingId, int userId)
        {
            try
            {
                await _bookingService.RemoveUserFromWaitingListAsync(bookingId, userId);
                return Ok(new { Message = "User removed from waiting list successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{bookingId}/add-to-participants")]
        public async Task<IActionResult> AddUserToParticipants(int bookingId, [FromBody] int userId)
        {
            try
            {
                await _bookingService.ApproveUserAsync(bookingId, userId);
                return Ok(new { Message = "User added to participants successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{bookingId}/remove-from-participants/{userId}")]
        public async Task<IActionResult> RemoveUserFromParticipants(int bookingId, int userId)
        {
            try
            {
                await _bookingService.RemoveUserFromApprovedListAsync(bookingId, userId);
                return Ok(new { Message = "User removed from participants successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("{bookingId}/waiting-list")]
        public async Task<IActionResult> GetWaitingList(int bookingId)
        {
            try
            {
                var users = await _bookingService.GetWaitingListByBookingIdAsync(bookingId);

                var response = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PictureUrl = user.PictureUrl,
                    RoleName = user.Role?.Name
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpGet("{bookingId}/approved-participants")]
        public async Task<IActionResult> GetApprovedParticipants(int bookingId)
        {
            try
            {
                var users = await _bookingService.GetApprovedParticipantsByBookingIdAsync(bookingId);

                var response = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PictureUrl = user.PictureUrl,
                    RoleName = user.Role?.Name
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
