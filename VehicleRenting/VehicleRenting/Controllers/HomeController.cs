﻿using System;
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

        public ActionResult Search()
        {
            var vehicleColorParam = Request.Form["vehiclecolor"];
            var availableToDoubleParam = Request.Form["availabletodouble"];
            var priceRangeParam = Request.Form["rentperweek"];

            var vehicles = db.Vehicles.Where(v => true);
            if (!string.IsNullOrWhiteSpace(vehicleColorParam))
            {
                vehicleColorParam = vehicleColorParam.ToLower();
                vehicles = vehicles.Where(v => v.VehicleColor.ToLower().Contains(vehicleColorParam));
            }

            if (!string.IsNullOrWhiteSpace(availableToDoubleParam))
            {
                var availableToDouble = availableToDoubleParam.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;
                vehicles = vehicles.Where(v => v.AvailableToDouble == availableToDouble);
            }

            if (!string.IsNullOrWhiteSpace(priceRangeParam))
            {
                int priceCategory = Convert.ToInt32(priceRangeParam);
                switch (priceCategory)
                {
                    case 1:
                        vehicles = vehicles.Where(v => v.RentPerWeek >= 0M && v.RentPerWeek <= 100M);
                        break;
                    case 2:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 200M);
                        break;
                    case 3:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 500M);
                        break;
                    case 4:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 1000M);
                        break;
                    case 5:
                        vehicles = vehicles.Where(v => v.RentPerWeek <= 2000M);
                        break;
                    case 6:
                        vehicles = vehicles.Where(v => v.RentPerWeek > 2000M);
                        break;
                    default:
                        vehicles = vehicles.Where(v => true);
                        break;
                }
            }

            var vehicleList = vehicles.ToList();
            return View(vehicleList);
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
                var vehicle = db.Vehicles.Find(vehicleId);
                var comments = $"I am interested in vehicle with registration number ${vehicle.RegistrationNo}, {vehicle.VehicleColor} color.";
                ViewBag.Comments = comments;

                return View(model);
            }
        }
    }
}