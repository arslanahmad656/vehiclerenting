using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VehicleRenting.Models
{
    public class DriverUserRegisterViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public int SalutationId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Rent Due Date")]
        public Nullable<int> RentDueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Rent Date")]
        public Nullable<System.DateTime> RentDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Contract From")]
        public Nullable<System.DateTime> ContractFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Contract To")]
        public Nullable<System.DateTime> ContractTo { get; set; }

        [Display(Name = "Contract Length")]
        public Nullable<int> ContractLength { get; set; }

        [Display(Name = "Source")]
        public Nullable<int> SourceId { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Security Deposit Amount")]
        public Nullable<decimal> SecurityDepositAmount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Advanced Rent Amount")]
        public Nullable<decimal> AdvancedRentAmount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Holding Deposit Amount")]
        public Nullable<decimal> HoldingDepositAmount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Admin Fee")]
        public Nullable<decimal> AdminFee { get; set; }

        [Display(Name = "Reference")]
        public Nullable<int> ReferenceId { get; set; }
        
        [Display(Name = "Reference Document")]
        public string ReferenceDocumentPath { get; set; }

        [Display(Name = "Identity")]
        public Nullable<int> IdentityId { get; set; }

        [Display(Name = "Identity Document")]
        public string IdentityDocuementPath { get; set; }
        
        [Display(Name = "Nationality")]
        public Nullable<int> NationalityId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Special Condition")]
        public string SpecialConditions { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone No.")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}