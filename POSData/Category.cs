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
    
    public partial class Category
    {
        public int ID { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<System.DateTime> CratedDate { get; set; }
        public string CratedUser { get; set; }
    }
}