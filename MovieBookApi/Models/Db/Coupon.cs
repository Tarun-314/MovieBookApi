using System;
using System.Collections.Generic;

namespace MovieBookApi.Models.Db;

public partial class Coupon
{
    public string CouponId { get; set; } = null!;

    public string CouponCode { get; set; } = null!;

    public decimal DiscountAmount { get; set; }
}
