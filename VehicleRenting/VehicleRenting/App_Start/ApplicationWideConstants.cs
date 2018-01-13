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
    }
}