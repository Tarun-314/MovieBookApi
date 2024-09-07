using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Services;
using WebApi2.Models;

namespace MovieBookApi.Controllers
{
    [Route("Statistics")]
    [ApiController]
    public class MovieStatisticsController : ControllerBase
    {
        private readonly IMovieStatisticsService? _movieStatisticsService;

        public MovieStatisticsController(IMovieStatisticsService movieStatisticsService)
        {
            _movieStatisticsService = movieStatisticsService;
        }

        //[Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("week/{multiplexName}")]
        public async Task<IActionResult> GetMoviesInWeek(string multiplexName, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var movies = await _movieStatisticsService.GetMoviesInWeekAsync(multiplexName, startDate, endDate);
            return Ok(movies);
        }

        //[Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("sales/{movieName}")]
        public async Task<IActionResult> GetTotalTicketSales(string movieName, [FromQuery] DateOnly month)
        {
            var totalSales = await _movieStatisticsService.GetTotalTicketSalesAsync(movieName, month);
            return Ok(totalSales);
        }

        //[Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("sales/quarter")]
        public async Task<IActionResult> GetSalesByQuarter([FromQuery] int year, [FromQuery] int quarter)
        {
            var sales = await _movieStatisticsService.GetSalesByQuarterAsync(year, quarter);
            return Ok(sales);
        }

        //[Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("movie-of-the-month")]
        public async Task<IActionResult> GetMovieOfTheMonth([FromQuery] DateOnly month)
        {
            var movie = await _movieStatisticsService.GetMovieOfTheMonthAsync(month);
            return Ok(movie);
        }

        //[Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("disaster-of-the-month")]
        public async Task<IActionResult> GetDisasterOfTheMonth([FromQuery] DateOnly month)
        {
            var movie = await _movieStatisticsService.GetDisasterOfTheMonthAsync(month);
            return Ok(movie);
        }
    }
}

