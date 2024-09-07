using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Review
{
    public string ReviewId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string MovieId { get; set; } = null!;

    public double? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? ReviewDate { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
