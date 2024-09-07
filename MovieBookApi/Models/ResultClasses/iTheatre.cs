namespace MovieBookApi.Models.ResultClasses
{
    public class iTheatre
    {
        public string Name { get; set; } = null!;

        public string Area { get; set; } = null!;

        public string Location { get; set; } = null!;

        public double Ratings { get; set; }

        public int? Screens { get; set; }

        public int TotalSeats { get; set; }

        public string Image { get; set; } = null!;

    }
}
public class uTheatre
{
    public string TheatreId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string Location { get; set; } = null!;

    public double Ratings { get; set; }

    public int? Screens { get; set; }

    public int TotalSeats { get; set; }

    public string Image { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }
}
