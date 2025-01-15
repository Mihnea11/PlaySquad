using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
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

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(bookings);
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
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            try
            {
                var createdBooking = await _bookingService.CreateBookingAsync(booking);
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking updatedBooking)
        {
            try
            {
                var booking = await _bookingService.UpdateBookingAsync(id, updatedBooking);
                return Ok(booking);
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
        public async Task<IActionResult> AddUserToWaitingList(int bookingId, [FromBody] User user)
        {
            try
            {
                await _bookingService.AddUserToWaitingListAsync(bookingId, user);
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
        public async Task<IActionResult> AddUserToParticipants(int bookingId, [FromBody] User user)
        {
            try
            {
                await _bookingService.ApproveUserAsync(bookingId, user.Id);
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
    }
}
