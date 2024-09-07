namespace MovieBookApi.Models.ResultClasses
{
    public class iUser
    {
        public string UserId {  get; set; }

        public string FullName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? SecurityQuestion { get; set; }

        public string? SecurityAnswer { get; set; }

    }
}
