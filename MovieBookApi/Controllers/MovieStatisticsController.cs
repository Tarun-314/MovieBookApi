using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using MovieBookApi.Services;
using System.Diagnostics.Metrics;
using WebApi2.Models;

namespace MovieBookApi.Controllers
{
    [Route("Statistics")]
    [ApiController]
    public class MovieStatisticsController : ControllerBase
    {
        private readonly IMovieStatisticsService service;

        public MovieStatisticsController(IMovieStatisticsService movieStatisticsService)
        {
            service = movieStatisticsService;
        }

        // Route for GetMovieCollections
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet("MovieCollections/{movieId}")]
        public async Task<ActionResult<List<MovieCollection>>> GetMovieCollections(string movieId)
        {
            var result = await service.GetMovieCollections(movieId);
            if (result == null || result.Count == 0)
                return NotFound(new DataTransferObject() {IsSuccess=false, Message="No collections found for the specified movie." });
            return Ok(result);
        }
        // Route for GetTheatreSales
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet("TheatreSales/{theatreId}")]
        public async Task<ActionResult<List<TheatreSales>>> GetTheatreSales(string theatreId)
        {
            var result = await service.GetTheatreSales(theatreId);
            if (result == null || result.Count == 0)
                return NotFound(new DataTransferObject() { IsSuccess = false, Message = "No sales found for the specified theatre." });
            return Ok(result);
        }

        // Route for GetMovieOfTheMonthAsync
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet("MOM/{month}")]
        public async Task<ActionResult<uMovie>> GetMovieOfTheMonth(DateOnly month)
        {
            var result = await service.GetMovieOfTheMonthAsync(month);
            if (result == null)
                return NotFound(new DataTransferObject() { IsSuccess = false, Message = "No movie of the month found." });
            return Ok(result);
        }

        // Route for GetDisasterOfTheMonthAsync
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet("DOM/{month}")]
        public async Task<ActionResult<uMovie>> GetDisasterOfTheMonth(DateOnly month)
        {
            var result = await service.GetDisasterOfTheMonthAsync(month);
            if (result == null)
                return NotFound(new DataTransferObject() { IsSuccess = false, Message = "No disaster movie of the month found." });
            return Ok(result);
        }
    }
}

