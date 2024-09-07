using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class TheatreMovie
{
    public string TheatreMovieId { get; set; } = null!;

    public string TheatreId { get; set; } = null!;

    public string MovieId { get; set; } = null!;

    public int ScreenNumber { get; set; }

    public DateOnly ShowDate { get; set; }

    public string ShowTimes { get; set; } = null!;

    public string AvailableSeats { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;

    public virtual Theatre Theatre { get; set; } = null!;
}
