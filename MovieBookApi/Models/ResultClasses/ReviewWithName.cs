namespace MovieBookApi.Models.ResultClasses
{
    public class ReviewWithUserName
    {
        public string ReviewID { get; set; }
        public string MovieID { get; set; }
        public string UserID { get; set; }
        public double? Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string UserName { get; set; }
    }
}
