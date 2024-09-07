using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? SecurityQuestion { get; set; }

    public string? SecurityAnswer { get; set; }

    public string? Role { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
