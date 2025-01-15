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
    public class SoccerFieldController : ControllerBase
    {
        private readonly ISoccerFieldService _soccerFieldService;

        public SoccerFieldController(ISoccerFieldService soccerFieldService)
        {
            _soccerFieldService = soccerFieldService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSoccerFields()
        {
            try
            {
                var soccerFields = await _soccerFieldService.GetAllSoccerFieldsAsync();
                return Ok(soccerFields);
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
                return Ok(soccerField);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSoccerField([FromBody] SoccerField soccerField)
        {
            try
            {
                var createdSoccerField = await _soccerFieldService.CreateSoccerFieldAsync(soccerField);
                return CreatedAtAction(nameof(GetSoccerFieldById), new { id = createdSoccerField.Id }, createdSoccerField);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSoccerField(int id, [FromBody] SoccerField updatedSoccerField)
        {
            try
            {
                var soccerField = await _soccerFieldService.UpdateSoccerFieldAsync(id, updatedSoccerField);
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

        [HttpPost("{soccerFieldId}/add-booking")]
        public async Task<IActionResult> AddBookingToSoccerField(int soccerFieldId, [FromBody] Booking booking)
        {
            try
            {
                await _soccerFieldService.AddBookingToSoccerFieldAsync(soccerFieldId, booking);
                return Ok(new { Message = "Booking added to soccer field successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{soccerFieldId}/remove-booking/{bookingId}")]
        public async Task<IActionResult> RemoveBookingFromSoccerField(int soccerFieldId, int bookingId)
        {
            try
            {
                await _soccerFieldService.RemoveBookingFromSoccerFieldAsync(soccerFieldId, bookingId);
                return Ok(new { Message = "Booking removed from soccer field successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
