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

        public static string GetIssueStatusString(bool? status)
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

        public static string GetVehicleRequestStatusString(bool? status)
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

        public static string GetVehicleHireStatusString(bool? status)
        {
            if(status == null)
            {
                return "NA";
            }
            else if(status == true)
            {
                return "Hired";
            }
            return "Available";
        }

        public static bool VehicleHired
        {
            get
            {
                return true;
            }
        }

        public static bool VehicleAvailable
        {
            get
            {
                return false;   
            }
        }

        public static string GetLiveryString(bool? status)
        {
            if(status == null)
            {
                return "NA";
            }
            else if(status == true)
            {
                return "Livery";
            }
            return "Non Livery";
        }

        public static int? GetLiveryInt(bool? status)
        {
            if (status == null)
            {
                return null;
            }
            else if (status == true)
            {
                return 1;
            }
            return 0;
        }

        public static bool Livery
        {
            get
            {
                return true;
            }
        }

        public static bool NonLivery
        {
            get
            {
                return false;
            }
        }
    }
}