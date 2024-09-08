using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Booking
{
    public string BookingId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string MovieId { get; set; } = null!;

    public string TheatreId { get; set; } = null!;

    public DateTime? BookingDate { get; set; }

    public DateOnly ShowDate { get; set; }

    public string ShowTime { get; set; } = null!;

    public int ScreenNumber { get; set; }

    public int NumberOfSeats { get; set; }

    public string Seats { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Theatre Theatre { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
