namespace MovieBookApi.Models.ResultClasses
{
    public class iReview
    {
        public string UserId { get; set; } = null!;

        public string MovieId { get; set; } = null!;

        public double? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
