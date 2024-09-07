namespace MovieBookApi.Models.ResultClasses
{
    public class TheatreofMovie
    {
        public string TheatreId { get; set; }
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public int ScreenNumber { get; set; }
        public double? Rating { get; set; }
        public string ShowTimes { get; set; }
        public string Image { get; set; }
    }
}
