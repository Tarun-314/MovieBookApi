namespace MovieBookApi.Models.ResultClasses
{
    public class UserCredentials
    {
        public string Useremail { get; set; }
        public string Password { get; set; }
        public string? Token {  get; set; }
        public string? SercurityQuestion {  get; set; }
    }
}
