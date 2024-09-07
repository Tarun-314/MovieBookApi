namespace MovieBookApi.Models.ResultClasses
{
    public class BookingData
    {
        public string UserId { get; set; }
        public string TheatreId { get; set; }
        public string MovieId { get; set; }
        public DateOnly SelectedDate { get; set; }
        public string SelectedTime { get; set; }
        public string Seats { get; set; }
        public int ScreenNumber { get; set; }
        public decimal Amount { get; set; }
        public string SeatString { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
    }

}
