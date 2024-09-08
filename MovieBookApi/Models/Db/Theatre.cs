using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Theatre
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

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<TheatreMovie> TheatreMovies { get; set; } = new List<TheatreMovie>();
}
