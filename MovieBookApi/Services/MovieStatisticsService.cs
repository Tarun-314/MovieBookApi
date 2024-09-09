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
        Task<List<MovieCollection>> GetMovieCollections(string movieid);
        Task<List<TheatreSales>> GetTheatreSales(string theatreid);
        Task<uMovie> GetMovieOfTheMonthAsync(DateOnly month);
        Task<uMovie> GetDisasterOfTheMonthAsync(DateOnly month);
    }

    public class MovieStatisticsService : IMovieStatisticsService
    {
        private readonly MovieBookDbContext context;

        public MovieStatisticsService(MovieBookDbContext context)
        {
            this.context = context;
        }

        public async Task<uMovie> GetDisasterOfTheMonthAsync(DateOnly month)
        {
            var movie = await context.Reviews
                .Where(r => r.ReviewDate.Value.Month == month.Month && r.ReviewDate.Value.Year == DateTime.Now.Year)
                .GroupBy(r => r.Movie)
                .Select(g => new
                {
                    Movie = g.Key,
                    AvgRating = g.Average(r => r.Rating)
                })
                .OrderBy(g => g.AvgRating)
                .Select(g => new uMovie()
                {
                    MovieId = g.Movie.MovieId,
                    Title = g.Movie.Title,
                    Genre = g.Movie.Genre,
                    Duration = g.Movie.Duration,
                    ReleaseDate = g.Movie.ReleaseDate,
                    Rating = g.AvgRating,
                    Likes = g.Movie.Likes,
                    Description = g.Movie.Description,
                    Casting = g.Movie.Casting,
                    Trailer = g.Movie.Trailer,
                    Language = g.Movie.Language,
                    Image = g.Movie.Image,
                    UpdatedAt = g.Movie.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (movie == null) return null;
            return new uMovie
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Genre = movie.Genre,
                Duration = movie.Duration,
                ReleaseDate =movie.ReleaseDate,
                Rating = movie.Rating,
                Likes = movie.Likes,
                Description = movie.Description,
                Casting = movie.Casting,
                Trailer = movie.Trailer,
                Language = movie.Language,
                Image = movie.Image,
                UpdatedAt = movie.UpdatedAt
            };
        }


        public async Task<List<MovieCollection>> GetMovieCollections(string movieid)
        {
            var collections = await context.Bookings
            .Where(b => b.MovieId == movieid)
            .GroupBy(b => new { b.TheatreId, b.Theatre.Name })
            .Select(g => new MovieCollection
            {
                TheatreName = g.Key.Name,
                TotalAmount = g.Sum(b => b.TotalPrice)
            })
            .ToListAsync();

            return collections;
        }

        public async Task<uMovie> GetMovieOfTheMonthAsync(DateOnly month)
        {
            var movie = await context.Reviews
                .Where(r => r.ReviewDate.Value.Month == month.Month && r.ReviewDate.Value.Year == DateTime.Now.Year)
                .GroupBy(r => r.Movie)
                .Select(g => new
                {
                    Movie = g.Key,
                    AvgRating = g.Average(r => r.Rating)
                })
                .OrderByDescending(g => g.AvgRating)
                .Select(g => new uMovie()
                {
                    MovieId = g.Movie.MovieId,
                    Title = g.Movie.Title,
                    Genre = g.Movie.Genre,
                    Duration = g.Movie.Duration,
                    ReleaseDate = g.Movie.ReleaseDate,
                    Rating = g.AvgRating,
                    Likes = g.Movie.Likes,
                    Description = g.Movie.Description,
                    Casting = g.Movie.Casting,
                    Trailer = g.Movie.Trailer,
                    Language = g.Movie.Language,
                    Image = g.Movie.Image,
                    UpdatedAt = g.Movie.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (movie == null) return null;
            return new uMovie
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Genre = movie.Genre,
                Duration = movie.Duration,
                ReleaseDate =movie.ReleaseDate,
                Rating = movie.Rating,
                Likes = movie.Likes,
                Description = movie.Description,
                Casting = movie.Casting,
                Trailer = movie.Trailer,
                Language = movie.Language,
                Image = movie.Image,
                UpdatedAt = movie.UpdatedAt
            };
        }


        public async Task<List<TheatreSales>> GetTheatreSales(string theatreId)
        {
            var sales = await context.Bookings
                .Where(b => b.TheatreId == theatreId && b.BookingDate.Value.Year == DateTime.Now.Year)
                .GroupBy(b => b.BookingDate.Value.Month)
                .Select(g => new TheatreSales
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(b => b.TotalPrice)
                })
                .ToListAsync();

            return sales;
        }

    }
}
