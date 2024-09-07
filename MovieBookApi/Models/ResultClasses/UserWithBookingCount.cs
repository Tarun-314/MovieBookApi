namespace MovieBookApi.Models.ResultClasses
{
    public class UserWithBookingCount
    {
        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Role { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int BookingCount { get; set; } 
    }

}
