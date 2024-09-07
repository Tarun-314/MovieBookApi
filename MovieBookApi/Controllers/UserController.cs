using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using MovieBookApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi2.Models;

namespace MovieBookApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            this.service = service;
            this.logger = logger;
        }
        /// <summary>
        /// Retrieves a list of all theaters.
        /// </summary>
        /// <returns>A list of all theaters.</returns>
        [Authorize]
        [HttpGet]
        [Route("AllTheaters")]
        public IActionResult AllTheaters()
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            logger.LogInformation(name);
            var list = service.GetAllTheatres();
            return Ok(list);
        }
        /// <summary>
        /// Retrieves a list of all movies.
        /// </summary>
        /// <returns>A list of all movies.</returns>
        [Authorize]
        [HttpGet]
        [Route("AllMovies")]
        public IActionResult AllMovies()
        {
            var list = service.GetAllMovies();
            return Ok(list);
        }
        /// <summary>
        /// Retrieves a list of all areas.
        /// </summary>
        /// <returns>A list of all areas.</returns>
        [Authorize]
        [HttpGet]
        [Route("Areas")]
        public IActionResult Areas()
        {
            var list = service.GetAreas();
            return Ok(list);
        }
        /// <summary>
        /// Retrieves theaters by the specified area.
        /// </summary>
        /// <param name="area">The area to filter theaters.</param>
        /// <returns>A list of theaters in the specified area.</returns>
        [Authorize]
        [HttpGet]
        [Route("TheatrebyArea/{area}")]
        public IActionResult GetTheatrebyArea(string area)
        {
            var list = service.GetTheatresByArea(area);
            return Ok(list);
        }
        /// <summary>
        /// Retrieves movies playing in a specified theater on a specific date.
        /// </summary>
        /// <param name="TheatreId">The ID of the theater.</param>
        /// <param name="date">The date to filter movies.</param>
        /// <returns>A list of movies playing in the specified theater on the given date.</returns>
        [Authorize]
        [HttpGet]
        [Route("TheatreMovies/{TheatreId}/{date}")]
        public IActionResult GetMoviesinTheater(string TheatreId, DateOnly date)
        {
            var list = service.GetMoviesinTheater(TheatreId, date);
            return Ok(list);
        }
        /// <summary>
        /// Retrieves theaters showing a specified movie on a specific date.
        /// </summary>
        /// <param name="MovieId">The ID of the movie.</param>
        /// <param name="date">The date to filter theaters.</param>
        /// <returns>A list of theaters showing the specified movie on the given date.</returns>
        [Authorize]
        [HttpGet]
        [Route("MoviesTheatre/{MovieId}/{date}/{city}")]
        public IActionResult GetTheaterofMovies(string MovieId, DateOnly date,string city)
        {
            var list = service.GetTheatreofMovies(MovieId, date,city);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves the seat availability for a specified show.
        /// </summary>
        /// <param name="theatre">The ID of the theater.</param>
        /// <param name="movie">The ID of the movie.</param>
        /// <param name="date">The date of the show.</param>
        /// <param name="showtime">The time of the show.</param>
        /// <returns>The seat availability for the specified show.</returns>
        [Authorize]
        [HttpGet]
        [Route("ShowSeatsGet/{theatre}/{movie}/{date}/{showtime}")]
        public IActionResult GetShowSeats(string theatre,string movie, DateOnly date,string showtime)
        {
            var seats = service.GetShowSeats(theatre,movie, date,showtime);
            return Ok(new DataTransferObject() { IsSuccess = true, Data = seats,Message="Seat string" });
        }
        /// <summary>
        /// Retrieves the discount amount for a specified coupon code.
        /// </summary>
        /// <param name="code">The coupon code.</param>
        /// <returns>The discount amount for the specified coupon code.</returns>
        [Authorize]
        [HttpGet]
        [Route("Coupon/{code}")]
        public IActionResult GetDiscount(string code)
        {
            var amt=service.GetDiscount(code);
            return Ok(amt);
        }
        /// <summary>
        /// Retrieves all reviews for a specified movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A list of reviews for the specified movie.</returns>
        [Authorize]
        [HttpGet]
        [Route("Reviews/{movieId}")]
        public IActionResult GetReviews(string movieId)
        {
            var list = service.GetReviews(movieId); 
            return Ok(list);
        }
        /// <summary>
        /// Retrieves a theater by its ID.
        /// </summary>
        /// <param name="theatreId">The ID of the theater.</param>
        /// <returns>The theater details.</returns>
        [Authorize]
        [HttpGet]
        [Route("TheatreById/{theatreId}")]
        public IActionResult GetTheatreById(string theatreId)
        {
            var theatres = service.GetTheatre(theatreId);
            return Ok(theatres);
        }
        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>The movie details.</returns>
        [Authorize]
        [HttpGet]
        [Route("MovieById/{movieId}")]
        public IActionResult GetMovieById(string movieId)
        {
            var movies = service.GetMovie(movieId);
            return Ok(movies);
        }
        /// <summary>
        /// Adds a new booking.
        /// </summary>
        /// <param name="booking">The booking details.</param>
        /// <returns>The ID of the added booking record.</returns>
        //[Authorize]
        [HttpPost]
        [Route("AddBooking")]
        public IActionResult AddBooking([FromBody] BookingData booking)
        {
            //userid
            booking.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var bookingId =  service.AddBooking(booking);
            if (bookingId != null)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "inserted booking data.",Data=bookingId }); // Return the BookingID of the added record
            }
            return BadRequest(new DataTransferObject() { IsSuccess = false, Message = "Invalid booking data." });
            //seat string
            //service.AddPaymentAndUpdateBooking(payment, bookingId);
            // return Ok("Payment added and booking updated successfully.");

        }
        /// <summary>
        /// Retrieves user details by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user details.</returns>
        [Authorize]
        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById()
        {
            var userId= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var user = service.GetUserById(userId);
            if (user.FullName !="")
            {
                return Ok(user);
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "User not found." });
        }
        /// <summary>
        /// Verifies if a user is eligible to add a review for a specified movie.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A result indicating whether the user can add a review.</returns>
        [Authorize]
        [HttpGet]
        [Route("CanAddReview")]
        public IActionResult CanAddReview([FromQuery] string movieId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var result = service.CanAddReview(userId, movieId);
            return Ok(result);
           
        }
        /// <summary>
        /// Adds a new review for a movie.
        /// </summary>
        /// <param name="review">The review details.</param>
        /// <returns>A confirmation message.</returns>
        [Authorize]
        [HttpPost]
        [Route("AddReview")]
        public IActionResult AddReview([FromBody] iReview review)
        {
            review.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            if(service.AddReview(review)) return Conflict(new DataTransferObject() { IsSuccess = false, Message = "DUP" });
            return Ok(new DataTransferObject() { IsSuccess = true, Message = "Review added successfully." });
            
        }

        /// <summary>
        /// Retrieves all bookings made by a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of bookings made by the specified user.</returns>
        [Authorize]
        [HttpGet]
        [Route("GetBookingsByUserId")]
        public IActionResult GetBookingsByUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var bookings = service.GetBookingsByUserId(userId);

            if (bookings != null && bookings.Any())
            {
                return Ok(bookings);
            }

            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "No bookings found for the specified user." });
        }
        /// <summary>
        /// Updates user details.
        /// </summary>
        /// <param name="updatedUser">The updated user details.</param>
        /// <returns>A confirmation message.</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody] iUser updatedUser)
        {
            updatedUser.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var result = service.UpdateUser(updatedUser);

            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "User updated successfully." });
            }
            return NotFound(   new DataTransferObject() { IsSuccess = false, Message = "User not found or update failed." });
        }






    }
}
