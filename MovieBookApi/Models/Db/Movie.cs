using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Movie
{
    public string MovieId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public int Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public double? Rating { get; set; }

    public int Likes { get; set; }

    public string Description { get; set; } = null!;

    public string Casting { get; set; } = null!;

    public string Trailer { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string Image { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<TheatreMovie> TheatreMovies { get; set; } = new List<TheatreMovie>();
}
