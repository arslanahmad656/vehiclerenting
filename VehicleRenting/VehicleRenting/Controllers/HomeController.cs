using System;
using System.IO;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using VehicleRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VehicleRenting.App_Start;
using System.Net;

namespace VehicleRenting.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();
        private Entities db = new Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string vehiclemake, string vehicletype, string liverytype, string rentperweek)
        {
            //var vehicleColorParam = Request.Form["vehiclecolor"];
            //var availableToDoubleParam = Request.Form["availabletodouble"];
            //var priceRangeParam = Request.Form["rentperweek"];

            
            var priceRangeParam = rentperweek;

            var vehicles = db.Vehicles.Where(v => true);

            if (!string.IsNullOrWhiteSpace(vehiclemake))
            {
                vehiclemake = vehiclemake.ToLower();
                vehicles = vehicles.Where(v => v.VehicleMake.ToLower().Contains(vehiclemake));
            }

            if (!string.IsNullOrWhiteSpace(vehicletype))
            {
                var vehicleTypeId = Convert.ToInt32(vehicletype);
                vehicles = vehicles.Where(v => v.VehicleTypeId == vehicleTypeId);
            }

            if(!string.IsNullOrWhiteSpace(liverytype))
            {
                var liveryInt = Convert.ToInt32(liverytype);
                var liveryBool = liveryInt == 1 ? true : false;
                vehicles = vehicles.Where(v => v.IsLivery == liveryBool);
            }

            if (!string.IsNullOrWhiteSpace(priceRangeParam))
            {
                int priceCategory = Convert.ToInt32(priceRangeParam);
                switch (priceCategory)
                {
                    case 1:
                        vehicles = vehicles.Where(v => v.RentPerWeek >= 0M && v.RentPerWeek <= 200M);
                        break;
                    case 2:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 500M);
                        break;
                    case 3:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 800M);
                        break;
                    case 4:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 1600M);
                        break;
                    case 5:
                        vehicles = vehicles.Where(v => v.RentPerWeek > 1600M);
                        break;
                    default:
                        break;
                }
            }

            var vehicleList = vehicles.ToList();
            return View(vehicleList);
        }

        public ActionResult ListAllVehicles()
        {
            var vehicleList = db.Vehicles.ToList();
            return View("Search", vehicleList);
        }

        public ActionResult BookAppointment(int vehicleId)
        {
            var vehicle = db.Vehicles.Find(vehicleId);
            var comments = $"I am interested in vehicle with registration number ${vehicle.RegistrationNo}, {vehicle.VehicleColor} color.";
            ViewBag.Comments = comments;
            ViewBag.VehicleId = vehicleId;
            return View();
        }

        [HttpPost]
        public ActionResult BookAppointment(Appointment model)
        {
            model.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Appointments.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                var vehicleId = Convert.ToInt32((Request.Form["vehicleId"]));
                ViewBag.VehicleId = vehicleId;
                var vehicle = db.Vehicles.Find(vehicleId);
                var comments = $"I am interested in vehicle with registration number {vehicle.RegistrationNo}, {vehicle.VehicleColor} color.";
                ViewBag.Comments = comments;

                return View(model);
            }
        }
    }
}