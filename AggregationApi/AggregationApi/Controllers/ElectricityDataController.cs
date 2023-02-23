using AggregationApi.Services.Abstractions;
using AggregationApi.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AggregationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityDataController : ControllerBase
    {
        private readonly ElectricityDataService _electricityDataService;

        public ElectricityDataController(ElectricityDataService electricityDataService, ILogger<ElectricityDataController> logger)
        {
            _electricityDataService = electricityDataService;
        }

        [HttpPost("download")]
        public async Task<IActionResult> DownloadData()
        {
            await _electricityDataService.DownloadDataAndStoreInDatabase();
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetData()
        {
            var data = await _electricityDataService.GetData();
            return Ok(data);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteData()
        {
            await _electricityDataService.DeleteData();
            return NoContent();
        }
    }
}
