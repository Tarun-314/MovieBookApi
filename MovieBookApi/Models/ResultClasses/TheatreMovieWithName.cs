namespace MovieBookApi.Models.ResultClasses
{
    public class TheatreMovieWithName
    {
        public string TheatreMovieId { get; set; } = null!;

        public string TheatreId { get; set; } = null!;

        public string MovieId { get; set; } = null!;

        public string MovieName { get; set; } = null!;
        
        public string TheatreName { get; set; } = null!;

        public int ScreenNumber { get; set; }

        public DateOnly ShowDate { get; set; }

        public string ShowTimes { get; set; } = null!;

        public string AvailableSeats { get; set; } = null!;
        public string Area { get; set; } = null!;


    }
}
