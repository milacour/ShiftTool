using Microsoft.AspNetCore.Mvc;
using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftTool.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftController(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        // Hent en vagt baseret på ID og returner den som en ShiftDTO
        [HttpGet("{shiftId}")]
        public async Task<ActionResult<ShiftDTO>> GetShiftByIdAsync(int shiftId)
        {
            var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (shift == null)
            {
                return NotFound("Vagt ikke fundet");
            }

            var shiftDto = ConvertToDTO(shift);
            return Ok(shiftDto);
        }

        // Hent alle tilgængelige vagter og returner dem som en liste af ShiftDTOs
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAvailableShiftsAsync()
        {
            var shifts = await _shiftRepository.GetAvailableShiftsAsync();
            var shiftDtos = shifts.Select(ConvertToDTO).ToList();
            return Ok(shiftDtos);
        }

        // Hent vagter sorteret efter prioritet og returner dem som en liste af ShiftDTOs
        [HttpGet("priority")]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetShiftsSortedByPriorityAsync()
        {
            var shifts = await _shiftRepository.GetShiftsSortedByPriorityAsync();
            var shiftDtos = shifts.Select(ConvertToDTO).ToList();
            return Ok(shiftDtos);
        }

        // Hent alle vagter og returner dem som en liste af ShiftDTOs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAllShiftsAsync()
        {
            var shifts = await _shiftRepository.GetAllShiftsAsync();
            var shiftDtos = shifts.Select(ConvertToDTO).ToList();
            return Ok(shiftDtos);
        }

        // Opret en ny vagt baseret på en ShiftDTO
        [HttpPost]
        public async Task<ActionResult> CreateShiftAsync([FromBody] ShiftDTO shiftDto)
        {
            var shift = ConvertFromDTO(shiftDto);
            await _shiftRepository.CreateShiftAsync(shift);
            return Ok("Vagt oprettet succesfuldt");
        }

        // Opdater en eksisterende vagt baseret på ID og ShiftDTO
        [HttpPut("{shiftId}")]
        public async Task<ActionResult> UpdateShiftAsync(int shiftId, [FromBody] ShiftDTO shiftDto)
        {
            var existingShift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (existingShift == null)
            {
                return NotFound("Vagt ikke fundet");
            }

            ConvertFromDTO(shiftDto, existingShift);
            await _shiftRepository.UpdateShiftAsync(existingShift);
            return Ok("Vagt opdateret succesfuldt");
        }

        // Slet en vagt baseret på ID
        [HttpDelete("{shiftId}")]
        public async Task<ActionResult> DeleteShiftAsync(int shiftId)
        {
            await _shiftRepository.DeleteShiftAsync(shiftId);
            return Ok("Vagt slettet succesfuldt");
        }

        // Konverter en Shift-model til en ShiftDTO
        private ShiftDTO ConvertToDTO(Shift shift)
        {
            return new ShiftDTO
            {
                ShiftId = shift.ShiftId,
                Title = shift.Title,
                Description = shift.Description,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Priority = shift.Priority,
                IsBooked = shift.IsBooked,
                Booking = shift.Booking
            };
        }

        // Konverter en ShiftDTO til en Shift-model
        private Shift ConvertFromDTO(ShiftDTO shiftDto)
        {
            return new Shift
            {
                ShiftId = shiftDto.ShiftId,
                Title = shiftDto.Title,
                Description = shiftDto.Description,
                StartDateTime = shiftDto.StartDateTime,
                EndDateTime = shiftDto.EndDateTime,
                Priority = shiftDto.Priority,
                IsBooked = shiftDto.IsBooked,
                Booking = shiftDto.Booking
            };
        }

        // Opdater en eksisterende Shift-model med værdier fra en ShiftDTO
        private void ConvertFromDTO(ShiftDTO shiftDto, Shift shift)
        {
            shift.Title = shiftDto.Title;
            shift.Description = shiftDto.Description;
            shift.StartDateTime = shiftDto.StartDateTime;
            shift.EndDateTime = shiftDto.EndDateTime;
            shift.Priority = shiftDto.Priority;
            shift.IsBooked = shiftDto.IsBooked;
            shift.Booking = shiftDto.Booking;
        }
    }
}
