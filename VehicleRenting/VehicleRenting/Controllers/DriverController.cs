﻿using System;
using System.Net;
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

namespace VehicleRenting.Controllers
{
    [Authorize(Roles = "driver")]
    public class DriverController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();
        private Entities db = new Entities();

        // GET: Driver
        public ActionResult Index()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriver = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First();
            //ViewBag.DriverName = $"{loggedInDriver.FirstName} {loggedInDriver.LastName}";
            var fullName = $"{loggedInDriver.FirstName} {loggedInDriver.LastName}";
            ViewBag.DriverName = fullName;
            return View();
        }

        public ActionResult ReportIssue()
        {
            ViewBag.IssueTypeId = new SelectList(db.IssueTypes, "Id", "Title");
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            ViewBag.DriverId = loggedInDriverId;
            return View();
        }

        [HttpPost]
        public ActionResult ReportIssue(Issue model)
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            model.DriverId = loggedInDriverId;
            if(ModelState.IsValid)
            {
                db.Issues.Add(model);
                db.SaveChanges();
                ViewBag.IssueId = model.Id;
                return View("IssueTicket");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields correctly.");
                ViewBag.IssueTypeId = new SelectList(db.IssueTypes, "Id", "Title", model.IssueTypeId);
                ViewBag.DriverId = loggedInDriverId;
                return View(model);
            }
        }

        public ActionResult ReportedIssuesList()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            var issuesRelatedToLoggedInDriver = db.Issues.Where(issue => issue.DriverId == loggedInDriverId).ToList();
            return View(issuesRelatedToLoggedInDriver);
        }

        public ActionResult IssueDetails(int id)
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            Issue model = null;
            try
            {
                model = db.Issues.Where(issue => issue.DriverId == loggedInDriverId).Where(issue => issue.Id == id).First();
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(model);
        }
    }
}