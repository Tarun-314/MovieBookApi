using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Payment
{
    public string PaymentId { get; set; } = null!;

    public string BookingId { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionId { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
