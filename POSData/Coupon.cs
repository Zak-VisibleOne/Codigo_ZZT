//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POSData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupon
    {
        public int ID { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string QRCodeImageUrl { get; set; }
        public Nullable<int> AvailableQty { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
    }
}