namespace MovieBookApi.Models.ResultClasses
{
    public class MoviesinTheatre
    {
        public string TheatreId { get; set; }
        public string MovieId { get; set; }
        public string Title { get; set; }
        public int ScreenNumber { get; set; }
        public string Language {  get; set; }
        public DateOnly ReleaseDate {  get; set; }
        public double? Rating { get; set; }
        public int Likes { get; set; }
        public string ShowTimes {  get; set; }
        public string Image { get; set; }
    }
}
