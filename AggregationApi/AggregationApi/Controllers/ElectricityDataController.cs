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
        private readonly ElectricityDataService _electricityDataProcessor;

        public ElectricityDataController(ElectricityDataService electricityDataProcessor)
        {
            _electricityDataProcessor = electricityDataProcessor;
        }
        [HttpPost("download")]
        public async Task<IActionResult> DownloadData()
        {
            var data = await _electricityDataProcessor.DownloadData();
            await _electricityDataProcessor.SaveData(data);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetData()
        {
            var data = await _electricityDataProcessor.GetData();
            return Ok(data);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteData()
        {
            await _electricityDataProcessor.DeleteData();
            return NoContent();
        }
    }
}
