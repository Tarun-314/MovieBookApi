namespace MovieBookApi.Models.ResultClasses
{
    public class iTheatreMovie
    {
        public string TheatreId { get; set; } = null!;

        public string MovieId { get; set; } = null!;

        public int ScreenNumber { get; set; }

        public DateOnly ShowDate { get; set; }

        public string ShowTimes { get; set; } = null!;

        public string AvailableSeats { get; set; } = null!;

    }
}
