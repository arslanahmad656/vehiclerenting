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
    
    public partial class Contract
    {
        public int Id { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string DocumentPath { get; set; }
        public string Notes { get; set; }
    
        public virtual Vehicle Vehicle { get; set; }
    }
}
