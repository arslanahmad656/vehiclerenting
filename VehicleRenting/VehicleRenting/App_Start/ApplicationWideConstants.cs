using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleRenting.App_Start
{
    public static class ApplicationWideConstants
    {
        public static string DriverReferenceDocumentName
        {
            get
            {
                return "driver_reference";
            }
        }

        public static string DriverIdentityTypeDocumentName
        {
            get
            {
                return "driver_identity";
            }
        }

        public static string RootStoragePath
        {
            get
            {
                return "~/Content/";
            }
        }

        public static string DriverRoleName
        {
            get
            {
                return "driver";
            }
        }

        public static string GetIssueStatus(bool? status)
        {
            if(status == null)
            {
                return "NA";
            }
            if(status == true)
            {
                return "Closed";
            }
            return "Pending";
        }

        public static bool IssueStatusPending  // Pending status is the one that is open
        {
            get
            {
                return false;
            }
        }

        public static bool IssueStatusClosed
        {
            get
            {
                return true;
            }
        }

        public static string GetVehicleRequestStatus(bool? status)
        {
            if(status == null)
            {
                return "NA";
            }
            if(status == true)
            {
                return "Closed";
            }
            return "Open/Pending";
        }

        public static bool VehicleRequestOpen
        {
            get
            {
                return false;
            }
        }

        public static bool VehicleRequestClosed
        {
            get
            {
                return true;
            }
        }
    }
}