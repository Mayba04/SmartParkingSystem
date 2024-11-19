using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Core.DTOs.ParkingSpot;
using SmartParkingSystem.Core.Interfaces;

namespace SmartParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpotController : Controller
    {
        private readonly IParkingSpotService _parkingSpotService;

        public ParkingSpotController(IParkingSpotService parkingSpotService)
        {
            _parkingSpotService = parkingSpotService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _parkingSpotService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _parkingSpotService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ParkingSpotCreateDTO model)
        {
            var response = await _parkingSpotService.AddAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ParkingSpotUpdateDTO model)
        {
            var response = await _parkingSpotService.UpdateAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _parkingSpotService.DeleteAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedParkingSpots([FromQuery] string? location, int page = 1, int pageSize = 10)
        {
            var response = await _parkingSpotService.GetPagedParkingSpotsAsync(location, page, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("parkingLot/{parkingLotId}/paged")]
        public async Task<IActionResult> GetParkingSpotsByParkingLot(int parkingLotId, [FromQuery] string? location, int page = 1, int pageSize = 10)
        {
            var response = await _parkingSpotService.GetPagedParkingSpotsByParkingLotAsync(parkingLotId, location, page, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("parkingLot/{parkingLotId}")]
        public async Task<IActionResult> GetAllByParkingLot(int parkingLotId)
        {
            var response = await _parkingSpotService.GetAllByParkingLotAsync(parkingLotId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
