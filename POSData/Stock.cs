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
    
    public partial class Stock
    {
        public int ID { get; set; }
        public string StockCode { get; set; }
        public string StockDescription { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<double> SalePrice { get; set; }
        public Nullable<double> PurchasePrice { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}