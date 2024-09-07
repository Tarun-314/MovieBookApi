using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookApi.Models.Db;
using MovieBookApi.Models.ResultClasses;

namespace MovieBookApi.Services
{
    public interface IMovieStatisticsService
    {
        Task<List<TheatreMovieWithName>> GetMoviesInWeekAsync(string multiplexName, DateOnly startDate, DateOnly endDate);
        Task<int> GetTotalTicketSalesAsync(string movieName, DateOnly month);
        Task<List<MovieSales>> GetSalesByQuarterAsync(int year, int quarter);
        Task<Movie> GetMovieOfTheMonthAsync(DateOnly month);
        Task<Movie> GetDisasterOfTheMonthAsync(DateOnly month);
    }

    public class MovieStatisticsService : IMovieStatisticsService
    {
        private readonly MovieBookDbContext context;

        public MovieStatisticsService(MovieBookDbContext context)
        {
            this.context = context;
        }

        private static string Getshows(string showTimeString)
        {
            var showTimesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(showTimeString);
            var showTimes = showTimesDict!.Keys.ToList();
            return string.Join(", ", showTimes);
        }

        public async Task<List<TheatreMovieWithName>> GetMoviesInWeekAsync(string multiplexName, DateOnly startDate, DateOnly endDate)
        {
            var theatreMovieDetails = from tm in context.TheatreMovies
                                      join movie in context.Movies on tm.MovieId equals movie.MovieId
                                      join theatre in context.Theatres on tm.TheatreId equals theatre.TheatreId
                                      where theatre.Name == multiplexName && tm.ShowDate >= startDate && tm.ShowDate <= endDate
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
                                          TheatreName = theatre.Name
                                      };

            return await theatreMovieDetails.ToListAsync();
        }

        public async Task<int> GetTotalTicketSalesAsync(string movieName, DateOnly month)
        {
            return await context.Bookings
                .Include(b => b.Movie)
                .Where(b => b.Movie.Title == movieName && b.ShowDate.Month == month.Month && b.ShowDate.Year == month.Year)
                .SumAsync(b => b.NumberOfSeats);
        }

        public async Task<List<MovieSales>> GetSalesByQuarterAsync(int year, int quarter)
        {
            var startMonth = (quarter - 1) * 3 + 1;
            var endMonth = startMonth + 2;

            var sales = from b in context.Bookings
                        join m in context.Movies on b.MovieId equals m.MovieId
                        where b.ShowDate.Year == year && b.ShowDate.Month >= startMonth && b.ShowDate.Month <= endMonth
                        group b by m.Title into g
                        select new MovieSales
                        {
                            MovieTitle = g.Key,
                            TotalSales = g.Sum(b => b.NumberOfSeats)
                        };

            return await sales.ToListAsync();
        }

        public async Task<Movie> GetMovieOfTheMonthAsync(DateOnly month)
        {
            var movie = await context.Bookings
                .Include(b => b.Movie)
                .Where(b => b.ShowDate.Month == month.Month && b.ShowDate.Year == month.Year)
                .GroupBy(b => b.Movie)
                .OrderByDescending(g => g.Key.Likes) // Order by likes first
                .ThenByDescending(g => g.Sum(b => b.NumberOfSeats)) // Then by number of seats if likes are the same
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return movie;
        }

        public async Task<Movie> GetDisasterOfTheMonthAsync(DateOnly month)
        {
            var movie = await context.Bookings
                .Include(b => b.Movie)
                .Where(b => b.ShowDate.Month == month.Month && b.ShowDate.Year == month.Year)
                .GroupBy(b => b.Movie)
                .OrderBy(g => g.Sum(b => b.NumberOfSeats))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return movie;
        }
    }

    public class MovieSales
    {
        public string MovieTitle { get; set; }
        public int TotalSales { get; set; }
    }

}
