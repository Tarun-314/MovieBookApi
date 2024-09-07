using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using System.Text.Json;

namespace MovieBookApi.Services
{
    public interface IAdminService
    {
        public List<uTheatre> AllTheaters();
        public List<uMovie> GetAllMovies();
        public List<TheatreMovieWithName> GetAllTheatreMovies();
        public List<UserWithBookingCount> GetAllUsers();
        public bool UpdateMovie( uMovie updatedMovie);
        public bool UpdateTheatre(uTheatre updatedTheatre);
        public bool UpdateTheatreMovie(TheatreMovieWithName updatedTheatreMovie);
        public bool DeleteMovie(string movieId);
        public bool DeleteTheatre(string theatreId);
        public bool DeleteTheatreMovie(string theatreMovieId);
        public bool DeleteUser(string userId);
        public void InsertMovie(iMovie movie);
        public void InsertTheatre(iTheatre theatre);
        public bool InsertTheatreMovie(iTheatreMovie theatreMovie);
        public bool ChangeUserRole(string userId, string role);
        public List<BookingHistory> GetAllBookings();
    }
    public class AdminService : IAdminService
    {

        private readonly MovieBookDbContext context;
        private ILogger<AdminService> logger;

        public AdminService(MovieBookDbContext context, ILogger<AdminService> logger)
        {

            this.context = context;
            this.logger = logger;

        }
        private static string Getshows(string showTimeString)
        {
            var showTimesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(showTimeString);
            var showTimes = showTimesDict!.Keys.ToList();
            return string.Join(", ", showTimes);
        }
        private static string PutShowstring(string ShowTimes)
        {
            var showTimesList = ShowTimes.Split(", ").ToList();

            // Create a new dictionary to hold the JSON object
            var newShowTimesDict = new Dictionary<string, string>();

            // Populate the dictionary with each showtime and a string of 80 "0"s
            foreach (var showTime in showTimesList)
            {
                newShowTimesDict[showTime] = new string('0', 80);
            }

            // Serialize the dictionary to a JSON string
            var jsonString = JsonSerializer.Serialize(newShowTimesDict);

            return jsonString;
        }
        public List<uTheatre> AllTheaters()
        {
            var list = context.Theatres.Select(b=>new uTheatre() {
                Area=b.Area,
                Image=b.Image,
                Location=b.Location,
                Name=b.Name,
                Ratings=b.Ratings,
                Screens=b.Screens,
                TheatreId=b.TheatreId,
                TotalSeats=b.TotalSeats,
                UpdatedAt=b.UpdatedAt
            }).ToList();
            return list;
        }

        public List<uMovie> GetAllMovies()
        {
            return context.Movies.Select(m=>new uMovie()
            {
                Casting=m.Casting,
                UpdatedAt=m.UpdatedAt,
                Image=m.Image,
                Description=m.Description,
                Duration=m.Duration,
                Genre=m.Genre,
                Language=m.Language,
                Likes=m.Likes,
                MovieId=m.MovieId,
                Rating=m.Rating,
                ReleaseDate=m.ReleaseDate,
                Title=m.Title,
                Trailer=m.Trailer
            }).ToList();
        }

        public List<TheatreMovieWithName> GetAllTheatreMovies()
        {
            var theatreMovieDetails = from tm in context.TheatreMovies
                                      join movie in context.Movies on tm.MovieId equals movie.MovieId
                                      join theatre in context.Theatres on tm.TheatreId equals theatre.TheatreId
                                      select new TheatreMovieWithName
                                      {
                                          TheatreMovieId = tm.TheatreMovieId,
                                          MovieId = tm.MovieId,
                                          TheatreId = tm.TheatreId,
                                          ScreenNumber = tm.ScreenNumber,
                                          ShowDate = tm.ShowDate,
                                          ShowTimes = Getshows(tm.ShowTimes),
                                          AvailableSeats = tm.AvailableSeats,
                                          MovieName = movie.Title,
                                          TheatreName = theatre.Name,
                                          Area=theatre.Area
                                      };

            return theatreMovieDetails.ToList();
        }


        public List<UserWithBookingCount> GetAllUsers()
        {
            var usersWithBookingCount = from user in context.Users
                                        join booking in context.Bookings on user.UserId equals booking.UserId into userBookings
                                        where user.Role != "Admin"
                                        select new UserWithBookingCount
                                        {
                                            UserId = user.UserId,
                                            FullName = user.FullName,
                                            Email = user.Email,
                                            PhoneNumber = user.PhoneNumber,
                                            Role = user.Role,
                                            UpdatedAt = user.UpdatedAt,
                                            BookingCount = userBookings.Count()
                                        };

            return usersWithBookingCount.ToList();
        }

        public bool UpdateMovie(uMovie updatedMovie)
        {
            var movie = context.Movies.FirstOrDefault(m => m.MovieId == updatedMovie.MovieId);
            if (movie == null)
            {
                return false; // Movie not found
            }

            // Update movie properties
            movie.Title = updatedMovie.Title;
            movie.Rating = updatedMovie.Rating;
            movie.Likes = updatedMovie.Likes;
            movie.Genre = updatedMovie.Genre;
            movie.Duration = updatedMovie.Duration;
            movie.ReleaseDate = updatedMovie.ReleaseDate;
            movie.Description = updatedMovie.Description;
            movie.Casting = updatedMovie.Casting;
            movie.Trailer = updatedMovie.Trailer;
            movie.Language = updatedMovie.Language;
            movie.Image = updatedMovie.Image;

            context.SaveChanges();
            return true;
        }

        public bool UpdateTheatre(uTheatre updatedTheatre)
        {
            var theatre = context.Theatres.FirstOrDefault(t => t.TheatreId == updatedTheatre.TheatreId);
            if (theatre == null)
            {
                return false; // Theatre not found
            }
            
            // Update theatre properties
            theatre.Name = updatedTheatre.Name;
            theatre.Area = updatedTheatre.Area;
            theatre.Ratings = updatedTheatre.Ratings;
            theatre.Location = updatedTheatre.Location;
            theatre.Screens = updatedTheatre.Screens;
            theatre.TotalSeats = updatedTheatre.TotalSeats;
            theatre.Image = updatedTheatre.Image;
            theatre.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool UpdateTheatreMovie(TheatreMovieWithName updatedTheatreMovie)
        {
            var existingTheatreMovie = context.TheatreMovies
                .FirstOrDefault(tm => tm.TheatreMovieId == updatedTheatreMovie.TheatreMovieId);

            if (existingTheatreMovie == null)
            {
                return false; // TheatreMovie not found
            }

            // Update the properties
            existingTheatreMovie.TheatreId = updatedTheatreMovie.TheatreId;
            existingTheatreMovie.MovieId = updatedTheatreMovie.MovieId;
            existingTheatreMovie.ScreenNumber = updatedTheatreMovie.ScreenNumber;
            existingTheatreMovie.ShowDate = updatedTheatreMovie.ShowDate;
            existingTheatreMovie.ShowTimes = PutShowstring(updatedTheatreMovie.ShowTimes);
            existingTheatreMovie.AvailableSeats = updatedTheatreMovie.AvailableSeats;

            context.SaveChanges();

            return true; // Update successful
        }

        public bool DeleteMovie(string movieId)
        {
            var movie = context.Movies.FirstOrDefault(m => m.MovieId == movieId);
            if (movie == null)
            {
                return false; // Movie not found
            }
            // Delete related TheatreMovies
            var theatreMovies = context.TheatreMovies.Where(tm => tm.MovieId == movieId).ToList();
            context.TheatreMovies.RemoveRange(theatreMovies);

            // Delete related Bookings
            var bookings = context.Bookings.Where(b => b.MovieId == movieId).ToList();

            // Retrieve the list of BookingIDs for the related bookings
            var bookingIds = bookings.Select(b => b.BookingId).ToList();

            // Delete related Payments based on the retrieved BookingIDs
            var payments = context.Payments.Where(p => bookingIds.Contains(p.BookingId)).ToList();
            context.Payments.RemoveRange(payments);

            // Delete the bookings
            context.Bookings.RemoveRange(bookings);

            // Delete related Reviews
            var reviews = context.Reviews.Where(r => r.MovieId == movieId).ToList();
            context.Reviews.RemoveRange(reviews);

            // Delete the movie itself
            context.Movies.Remove(movie);
            context.SaveChanges();

            return true;
        }

        public bool DeleteTheatre(string theatreId)
        {
            var theatre = context.Theatres.FirstOrDefault(t => t.TheatreId == theatreId);
            if (theatre == null)
            {
                return false; // Theatre not found
            }
            // Delete related TheatreMovies
            var theatreMovies = context.TheatreMovies.Where(tm => tm.TheatreId == theatreId).ToList();
            context.TheatreMovies.RemoveRange(theatreMovies);

            // Delete related Bookings
            var bookings = context.Bookings.Where(b => b.TheatreId == theatreId).ToList();

            // Retrieve the list of BookingIDs for the related bookings
            var bookingIds = bookings.Select(b => b.BookingId).ToList();

            // Delete related Payments based on the retrieved BookingIDs
            var payments = context.Payments.Where(p => bookingIds.Contains(p.BookingId)).ToList();
            context.Payments.RemoveRange(payments);

            // Delete the bookings
            context.Bookings.RemoveRange(bookings);


            // Delete the theatre itself
            context.Theatres.Remove(theatre);
            context.SaveChanges();
            return true;
        }

        public bool DeleteTheatreMovie(string theatreMovieId)
        {
            var theatreMovie = context.TheatreMovies
                .FirstOrDefault(tm => tm.TheatreMovieId == theatreMovieId);

            if (theatreMovie == null)
            {
                return false; // TheatreMovie not found
            }

            context.TheatreMovies.Remove(theatreMovie);
            context.SaveChanges();

            return true; // Deletion successful
        }


        public void InsertMovie(iMovie movie)
        {
            var titleParam = new SqlParameter("@Title", movie.Title);
            var genreParam = new SqlParameter("@Genre", movie.Genre);
            var durationParam = new SqlParameter("@Duration", movie.Duration);
            var releaseDateParam = new SqlParameter("@ReleaseDate", movie.ReleaseDate);
            var ratingParam = new SqlParameter("@Rating", movie.Rating);
            var likesParam = new SqlParameter("@Likes", movie.Likes);
            var descriptionParam = new SqlParameter("@Description", movie.Description);
            var castingParam = new SqlParameter("@Casting", movie.Casting);
            var trailerParam = new SqlParameter("@Trailer", movie.Trailer);
            var languageParam = new SqlParameter("@Language", movie.Language);
            var imageParam = new SqlParameter("@Image", movie.Image);

            context.Database.ExecuteSqlRaw("EXEC InsertMovie @Title, @Genre, @Duration, @ReleaseDate, @Rating, @Likes, @Description, @Casting, @Trailer, @Language, @Image",
                titleParam, genreParam, durationParam, releaseDateParam, ratingParam, likesParam, descriptionParam, castingParam, trailerParam, languageParam, imageParam);
        }

        public void InsertTheatre(iTheatre theatre)
        {
            var nameParam = new SqlParameter("@Name", theatre.Name);
            var areaParam = new SqlParameter("@Area", theatre.Area);
            var locationParam = new SqlParameter("@Location", theatre.Location);
            var ratingsParam = new SqlParameter("@Ratings", theatre.Ratings);
            var screensParam = new SqlParameter("@Screens", theatre.Screens);
            var totalSeatsParam = new SqlParameter("@TotalSeats", theatre.TotalSeats);
            var imageParam = new SqlParameter("@Image", theatre.Image);

            context.Database.ExecuteSqlRaw("EXEC InsertTheatre @Name, @Area, @Location, @Ratings, @Screens, @TotalSeats, @Image",
                nameParam, areaParam, locationParam, ratingsParam, screensParam, totalSeatsParam, imageParam);
        }

        public bool InsertTheatreMovie(iTheatreMovie theatreMovie)
        {
            // Check if the screen is already booked on the specified date in the specified theatre
            bool screenExists = context.TheatreMovies
                .Any(tm => tm.TheatreId == theatreMovie.TheatreId &&
                           tm.ScreenNumber == theatreMovie.ScreenNumber &&
                           tm.ShowDate == theatreMovie.ShowDate);

            if (screenExists)
            {
                // Screen is already booked on that date
                return false;
            }

            // Proceed to insert the new TheatreMovie record if validation passes
            var theatreIdParam = new SqlParameter("@TheatreID", theatreMovie.TheatreId);
            var movieIdParam = new SqlParameter("@MovieID", theatreMovie.MovieId);
            var screenNumberParam = new SqlParameter("@ScreenNumber", theatreMovie.ScreenNumber);
            var showDateParam = new SqlParameter("@ShowDate", theatreMovie.ShowDate);
            var showTimesParam = new SqlParameter("@ShowTimes", PutShowstring(theatreMovie.ShowTimes));
            var availableSeatsParam = new SqlParameter("@AvailableSeats", theatreMovie.AvailableSeats);

            context.Database.ExecuteSqlRaw("EXEC InsertTheatreMovie @TheatreID, @MovieID, @ScreenNumber, @ShowDate, @ShowTimes, @AvailableSeats",
                theatreIdParam, movieIdParam, screenNumberParam, showDateParam, showTimesParam, availableSeatsParam);

            return true;
        }

        public bool ChangeUserRole(string userId, string role)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return false; // User not found
            }

            user.Role = role;
            context.SaveChanges();

            return true; // Role updated successfully
        }

        public bool DeleteUser(string userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return false; // User not found
            }
            // Delete related Bookings
            var bookings = context.Bookings.Where(b => b.UserId == userId).ToList();

            // Retrieve the list of BookingIDs for the related bookings
            var bookingIds = bookings.Select(b => b.BookingId).ToList();

            // Delete related Payments based on the retrieved BookingIDs
            var payments = context.Payments.Where(p => bookingIds.Contains(p.BookingId)).ToList();
            context.Payments.RemoveRange(payments);

            // Delete the bookings
            context.Bookings.RemoveRange(bookings);
           
            // Delete related Reviews
            var reviews = context.Reviews.Where(r => r.UserId == userId).ToList();
            context.Reviews.RemoveRange(reviews);

           

            // Delete the user itself
            context.Users.Remove(user);
            context.SaveChanges();

            return true; // User deleted successfully
        }

        public List<BookingHistory> GetAllBookings()
        {
            var bookingHistory = (from booking in context.Bookings
                                  join movie in context.Movies on booking.MovieId equals movie.MovieId
                                  join theatre in context.Theatres on booking.TheatreId equals theatre.TheatreId
                                  join payment in context.Payments on booking.BookingId equals payment.BookingId
                                  select new BookingHistory
                                  {
                                      UserID = booking.UserId,
                                      MovieName = movie.Title,
                                      TheatreName = theatre.Name,
                                      BookingDate = booking.BookingDate,
                                      ShowDate = booking.ShowDate,
                                      ShowTime = booking.ShowTime,
                                      Seats = booking.Seats,
                                      TotalPrice = booking.TotalPrice,
                                      TransactionID = payment.TransactionId,
                                      PaymentMethod = payment.PaymentMethod
                                  }).ToList();

            return bookingHistory;
        }
    }
}
