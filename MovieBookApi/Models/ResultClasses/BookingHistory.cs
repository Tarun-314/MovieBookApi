namespace MovieBookApi.Models.ResultClasses
{
    public class BookingHistory
    {
        public string UserID { get; set; }
        public string MovieName { get; set; }
        public string TheatreName { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateOnly ShowDate { get; set; }
        public string ShowTime { get; set; }
        public string Seats { get; set; }
        public decimal TotalPrice { get; set; }
        public string TransactionID { get; set; }
        public string PaymentMethod { get; set; }
    }
}
