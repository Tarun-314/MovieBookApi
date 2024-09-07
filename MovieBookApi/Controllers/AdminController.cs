using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using MovieBookApi.Services;
using System.Security.Claims;
using WebApi2.Models;

namespace MovieBookApi.Controllers
{
    [Route("Admin/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService Service;
        public AdminController(ILogger<AdminController> logger, IAdminService service)
        {
            this.Service = service;
            _logger = logger;
        }
        /// <summary>
        /// Gets a list of all theaters.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of theaters.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("AllTheaters")]
        public IActionResult GetAllTherters()
        {
            var list = Service.AllTheaters();
            return Ok(list);
        }
        /// <summary>
        /// Gets a list of all movies.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of movies.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("AllMovies")]
        public IActionResult GetAllMovies()
        {
            var movies = Service.GetAllMovies();
            return Ok(movies);
        }
        /// <summary>
        /// Gets a list of all theater-movie mappings.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of theater-movie mappings.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("GetAllTheatreMovies")]
        public IActionResult GetAllTheatreMovies()
        {
            var theatreMovies = Service.GetAllTheatreMovies();
            return Ok(theatreMovies);
        }
        /// <summary>
        /// Updates the details of a movie.
        /// </summary>
        /// <param name="updatedMovie">The updated movie details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPut]
        [Route("UpdateMovie")]
        public IActionResult UpdateMovie([FromBody] uMovie updatedMovie)
        {
            var result = Service.UpdateMovie( updatedMovie);
            if (result)
            {
                return Ok(new DataTransferObject() {IsSuccess=true, Message="Movie updated successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "Movie not found." });
        }
        /// <summary>
        /// Updates the details of a theater.
        /// </summary>
        /// <param name="updatedTheatre">The updated theater details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPut]
        [Route("UpdateTheatre")]
        public IActionResult UpdateTheatre([FromBody] uTheatre updatedTheatre)
        {
            var result = Service.UpdateTheatre( updatedTheatre);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "Theatre updated successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "Theatre not found." });
        }
        /// <summary>
        /// Updates the details of a theater-movie mapping.
        /// </summary>
        /// <param name="theatreMovie">The updated theater-movie mapping details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPut]
        [Route("UpdateTheatreMovie")]
        public IActionResult UpdateTheatreMovie([FromBody] TheatreMovieWithName theatreMovie)
        {
            var result = Service.UpdateTheatreMovie(theatreMovie);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "TheatreMovie updated successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "TheatreMovie not found." });
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpDelete]
        [Route("DeleteMovie/{movieId}")]
        public IActionResult DeleteMovie(string movieId)
        {
            var result = Service.DeleteMovie(movieId);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "Movie deleted successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "Movie not found." });
        }
        /// <summary>
        /// Deletes a theater by its ID.
        /// </summary>
        /// <param name="theatreId">The ID of the theater to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpDelete]
        [Route("DeleteTheatre/{theatreId}")]
        public IActionResult DeleteTheatre(string theatreId)
        {
            var result = Service.DeleteTheatre(theatreId);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "Theatre deleted successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "Theatre not found." });
        }
        /// <summary>
        /// Deletes a theater-movie mapping by its ID.
        /// </summary>
        /// <param name="theatreMovieId">The ID of the theater-movie mapping to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpDelete]
        [Route("DeleteTheatreMovie/{theatreMovieId}")]
        public IActionResult DeleteTheatreMovie(string theatreMovieId)
        {
            var result = Service.DeleteTheatreMovie(theatreMovieId);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "TheatreMovie deleted successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "TheatreMovie not found." });
        }
        /// <summary>
        /// Inserts a new movie into the system.
        /// </summary>
        /// <param name="movie">The movie details to insert.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the insert operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPost]
        [Route("InsertMovie")]
        public IActionResult InsertMovie([FromBody] iMovie movie)
        {
            Service.InsertMovie(movie);
            return Ok(new DataTransferObject() { IsSuccess = true, Message = "Movie inserted successfully." });
        }
        /// <summary>
        /// Inserts a new theater into the system.
        /// </summary>
        /// <param name="theatre">The theater details to insert.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the insert operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPost]
        [Route("InsertTheatre")]
        public IActionResult InsertTheatre([FromBody] iTheatre theatre)
        {
            Service.InsertTheatre(theatre);
            return Ok(new DataTransferObject() { IsSuccess = true, Message = "Theatre inserted successfully." });
        }
        /// <summary>
        /// Inserts a new theater-movie mapping into the system.
        /// </summary>
        /// <param name="theatreMovie">The theater-movie mapping details to insert.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the insert operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPost]
        [Route("InsertTheatreMovie")]
        public IActionResult InsertTheatreMovie([FromBody] iTheatreMovie theatreMovie)
        {
            var result = Service.InsertTheatreMovie(theatreMovie);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "TheatreMovie inserted successfully." });
            }
            return BadRequest(new DataTransferObject() { IsSuccess = false, Message = "The screen is already booked on the specified date." });
        }
        /// <summary>
        /// Blocks a user by setting their role to "Block".
        /// </summary>
        /// <param name="userId">The ID of the user to block.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the block operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPost]
        [Route("BlockUser/{userId}")]
        public IActionResult BlockUser(string userId)
        {
            var result = Service.ChangeUserRole(userId, "Block");
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "User blocked successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "User not found." });
        }
        /// <summary>
        /// Unblocks a user by setting their role to "User".
        /// </summary>
        /// <param name="userId">The ID of the user to unblock.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the unblock operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpPost]
        [Route("UnblockUser/{userId}")]
        public IActionResult UnblockUser(string userId)
        {
            var result = Service.ChangeUserRole(userId, "User");
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "User unblocked successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "User not found." });
        }
        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            var result = Service.DeleteUser(userId);
            if (result)
            {
                return Ok(new DataTransferObject() { IsSuccess = true, Message = "User deleted successfully." });
            }
            return NotFound(new DataTransferObject() { IsSuccess = false, Message = "User not found." });
        }
        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of users.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = Service.GetAllUsers();
            return Ok(users);
        }
        /// <summary>
        /// Gets a list of all Bookings.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of Bookings.</returns>
        [Authorize(Policy = SecurityPolicy.Admin)]
        [HttpGet]
        [Route("GetAllBookings")]
        public IActionResult GetAllBookings()
        {
            var Bookings = Service.GetAllBookings();
            return Ok(Bookings);
        }


    }
}
