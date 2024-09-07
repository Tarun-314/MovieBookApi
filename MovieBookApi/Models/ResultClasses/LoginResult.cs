namespace MovieBookApi.Models.ResultClasses
{
    public class LoginResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Role {  get; set; }
        public string Name { get; set; }
        public string Token {  get; set; }

    }
}
