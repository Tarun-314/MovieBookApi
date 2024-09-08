namespace MovieBookApi.Models.ResultClasses
{
    public class MovieCollection
    {
        public string TheatreName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }

    public class TheatreSales
    {
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
