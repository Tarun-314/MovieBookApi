using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieBookApi.Services
{
    public interface IUserService
    {
        public List<Theatre> GetAllTheatres();
        public List<Movie> GetAllMovies();
        public List<Theatre> GetTheatre(string theatreId);
        public List<Movie> GetMovie(string movieId);
        public List<string> GetAreas();
        public List<Theatre> GetTheatresByArea(string area);
        public List<MoviesinTheatre> GetMoviesinTheater(string theatreId, DateOnly date);
        public string GetShowSeats(string theatreId, string movieId, DateOnly showDate, string showTime);
        public decimal GetDiscount(string code);
        public List<TheatreofMovie> GetTheatreofMovies(string movieId, DateOnly showDate, string city);
        public bool PutShowSeats(string theatreId, string movieId, DateOnly showDate, string showtime, string seatString);
        public List<ReviewWithUserName> GetReviews(string movieId);
        public string AddBooking(BookingData booking);
        public iUser GetUserById(string userId);
        public bool CanAddReview(string userId, string movieId);
        public bool AddReview(iReview review);
        public List<BookingHistory> GetBookingsByUserId(string userId);
        public bool UpdateUser(iUser updatedUser);

    }
    public class UserService : IUserService
    {
        private readonly MovieBookDbContext context;
        private readonly ILogger<UserService> logger;

        private static List<string> Getshows(string showTimeString)
        {
            var showTimesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(showTimeString);
            var showTimes = showTimesDict!.Keys.ToList();
            return showTimes;
        }

        public UserService(MovieBookDbContext context, ILogger<UserService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public List<Movie> GetAllMovies()
        {
            return context.Movies.ToList();
        }

        public List<Theatre> GetAllTheatres()
        {
            return context.Theatres.ToList();
        }

        public List<string> GetAreas()
        {
            var result = (from t in context.Theatres group t by t.Area into temp select temp.Key).ToList();
            return result;
        }

        public List<MoviesinTheatre> GetMoviesinTheater(string theatreId, DateOnly date)
        {

            var res = (from mv in context.Movies
                       join thmv in context.TheatreMovies on mv.MovieId equals thmv.MovieId
                       where thmv.TheatreId == theatreId && thmv.ShowDate == date
                       select new MoviesinTheatre()
                       {
                           TheatreId = thmv.TheatreId,
                           MovieId = thmv.MovieId,
                           Image = mv.Image,
                           Title = mv.Title,
                           ScreenNumber = thmv.ScreenNumber,
                           ReleaseDate = mv.ReleaseDate,
                           Likes = mv.Likes,
                           Rating = mv.Rating,
                           Language = mv.Language,
                           ShowTimes = thmv.ShowTimes
                       }).ToList();
            return res;
        }

        public List<Theatre> GetTheatresByArea(string area)
        {
            return context.Theatres.Where(t => t.Area == area).ToList();
        }
        public string GetShowSeats(string theatreId, string movieId, DateOnly showDate, string showTime)
        {


            var showTimeString = context.TheatreMovies
                                        .Where(tm => tm.TheatreId == theatreId
                                                     && tm.MovieId == movieId
                                                     && tm.ShowDate == showDate)
                                        .Select(tm => tm.ShowTimes)
                                        .FirstOrDefault();

            if (string.IsNullOrEmpty(showTimeString))
            {
                return null;
            }


            var showTimes = JsonSerializer.Deserialize<Dictionary<string, string>>(showTimeString);

            if (showTimes!.ContainsKey(showTime))
            {
                return showTimes[showTime];
            }

            return null;
        }

        public decimal GetDiscount(string code)
        {
            var DiscountAmount = context.Coupons.Where(c => c.CouponCode == code).Select(c => c.DiscountAmount).FirstOrDefault();
            return DiscountAmount;
        }

        public List<TheatreofMovie> GetTheatreofMovies(string movieId, DateOnly showDate,string city)
        {
            var res = (from th in context.Theatres
                       join thmv in context.TheatreMovies on th.TheatreId equals thmv.TheatreId
                       where thmv.MovieId == movieId && thmv.ShowDate == showDate && th.Area == city
                       select new TheatreofMovie()
                       {
                           TheatreId = thmv.TheatreId,
                           MovieId = thmv.MovieId,
                           Image = th.Image,
                           Name = th.Name,
                           ScreenNumber = thmv.ScreenNumber,
                           Area = th.Area,
                           Location = th.Location,
                           Rating = th.Ratings,
                           ShowTimes =thmv.ShowTimes
                       }).ToList();
            return res;
        }
       
        public bool PutShowSeats(string theatreId, string movieId, DateOnly showDate, string showtime, string seatString)
        {
            logger.LogInformation(showDate.ToString());
            var theatreMovie = context.TheatreMovies
           .FirstOrDefault(tm => tm.TheatreId == theatreId
                                 && tm.MovieId == movieId
                                 && tm.ShowDate == showDate);

            if (theatreMovie != null)
            {
                // Parse the ShowTimes JSON string into a dictionary
                var showTimesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(theatreMovie.ShowTimes);
                if (showTimesDict != null && showTimesDict.ContainsKey(showtime))
                {
                    // Update the specific showtime with the new seat string
                    showTimesDict[showtime] = seatString;

                    // Convert the dictionary back to a JSON string
                    theatreMovie.ShowTimes = JsonSerializer.Serialize(showTimesDict);

                    // Save changes to the database
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<ReviewWithUserName> GetReviews(string movieId)
        {
            var reviewsWithUserName =(from r in context.Reviews join u in context.Users on r.UserId equals u.UserId
                                      where r.MovieId==movieId select new ReviewWithUserName()
                                      {
                                          Comment=r.Comment,
                                          MovieID=movieId,
                                          Rating=r.Rating,
                                          ReviewDate=r.ReviewDate,
                                          ReviewID=r.ReviewId,
                                          UserID=r.UserId,
                                          UserName= u.FullName
                                      }).ToList();

            return reviewsWithUserName;
        }


        public List<Theatre> GetTheatre(string theatreId)
        {
            return context.Theatres.Where(t => t.TheatreId == theatreId).Select(t => t).ToList();
        }

        public List<Movie> GetMovie(string movieId)
        {
            return context.Movies.Where(m => m.MovieId == movieId).Select(m=> m).ToList();
        }

        public  string AddBooking(BookingData booking)
        {
            if (context.Payments.Any(c => c.TransactionId == booking.TransactionId))
            {
                var bookings = (from b in context.Bookings join p in context.Payments on b.BookingId equals p.BookingId where p.TransactionId==booking.TransactionId select b.BookingId);
                return bookings.FirstOrDefault();
            }

            PutShowSeats(booking.TheatreId, booking.MovieId, booking.SelectedDate, booking.SelectedTime, booking.SeatString);
            ///putseatstring need to add
            var newBookingId = new SqlParameter
            {
                ParameterName = "@NewBookingID",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size=36
            };
            var NumberOfSeats =booking.Seats.Count(c=>c==',')+1;
            // Define the SQL command to call the stored procedure
            var sql = "EXEC InsertBooking @UserID, @MovieID, @TheatreID, @ShowDate, @ShowTime, @ScreenNumber, @NumberOfSeats, @Seats,@Status, @TotalPrice,@PaymentMethod,@NewBookingID OUTPUT";

            // Execute the stored procedure with parameters
            context.Database.ExecuteSqlRaw(
                sql,
                new SqlParameter("@UserID", booking.UserId),
                new SqlParameter("@MovieID", booking.MovieId),
                new SqlParameter("@TheatreID", booking.TheatreId),
                new SqlParameter("@ShowDate", booking.SelectedDate),
                new SqlParameter("@ShowTime", booking.SelectedTime),
                new SqlParameter("@ScreenNumber", booking.ScreenNumber),
                new SqlParameter("@NumberOfSeats", NumberOfSeats),
                new SqlParameter("@Seats", booking.Seats),
                new SqlParameter("@Status","Confirmed"),
                new SqlParameter("@TotalPrice", booking.Amount),
                new SqlParameter("@PaymentMethod", booking.PaymentMethod),
                newBookingId

            );
            context.Database.ExecuteSqlRaw(
                "EXEC InsertPayment @BookingID, @Amount, @PaymentMethod, @TransactionID",
                new SqlParameter("@BookingID", newBookingId.SqlValue.ToString()),
                new SqlParameter("@Amount", booking.Amount),
                new SqlParameter("@PaymentMethod", booking.PaymentMethod),
                new SqlParameter("@TransactionID", booking.TransactionId)
            );

            // Retrieve the newly inserted BookingID


            return newBookingId.SqlValue.ToString();
        }

        public iUser GetUserById(string userId)
        {
            
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            return new iUser() { Email=user.Email,FullName=user.FullName,PasswordHash=user.PasswordHash,PhoneNumber=user.PhoneNumber,SecurityAnswer=user.SecurityAnswer,SecurityQuestion=user.SecurityQuestion,UserId=user.UserId};
        }
        public bool CanAddReview(string userId, string movieId)
        {
            // Check if user has a confirmed booking for the movie with a past show date
            var hasValidBooking = context.Bookings.Any(b =>
                b.UserId == userId &&
                b.MovieId == movieId &&
                b.Status == "Confirmed" &&
                b.ShowDate < DateOnly.FromDateTime(DateTime.Now));

            return hasValidBooking;
        }

        public bool AddReview(iReview review)
        {
            var Alredyreview=context.Reviews.Any(b=>b.UserId == review.UserId && b.MovieId==review.MovieId);
            if (Alredyreview) return true;
            context.Database.ExecuteSqlRaw(
               "EXEC InsertReview @UserID, @MovieID, @Rating, @Comment",
               new SqlParameter("@UserID", review.UserId),
               new SqlParameter("@MovieID", review.MovieId),
               new SqlParameter("@Rating", review.Rating),
               new SqlParameter("@Comment", review.Comment)
           );
            return false;
        }

        public List<BookingHistory> GetBookingsByUserId(string userId)
        {
           
            var bookings = (from booking in context.Bookings
                           join movie in context.Movies on booking.MovieId equals movie.MovieId
                           join theatre in context.Theatres on booking.TheatreId equals theatre.TheatreId
                           join payment in context.Payments on booking.BookingId equals payment.BookingId
                           where booking.UserId==userId
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



            return bookings;
        }

        public bool UpdateUser(iUser updatedUser)
        {
            if (updatedUser == null || string.IsNullOrEmpty(updatedUser.UserId))
            {
                return false; // Invalid user data
            }

            var existingUser = context.Users.Find(updatedUser.UserId);

            if (existingUser == null)
            {
                return false; // User not found
            }

            // Update user properties
            existingUser.FullName = updatedUser.FullName ?? existingUser.FullName;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.SecurityQuestion = updatedUser.SecurityQuestion ?? existingUser.SecurityQuestion;
            existingUser.SecurityAnswer = updatedUser.SecurityAnswer ?? existingUser.SecurityAnswer;
            existingUser.UpdatedAt = DateTime.Now;

            // Save changes to the database
            context.SaveChanges();

            return true; // User updated successfully
        }


    }

}
