//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VehicleRenting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notice
    {
        public int Id { get; set; }
        public System.DateTime NoticeDate { get; set; }
        public System.DateTime CheckoutDate { get; set; }
        public string Reason { get; set; }
        public int DriverId { get; set; }
    
        public virtual Driver Driver { get; set; }
    }
}