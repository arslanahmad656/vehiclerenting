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
            ViewBag.VehicleId = new SelectList(db.Vehicles.Where(vehicle => vehicle.UnderOffer == null || vehicle.UnderOffer == false), "Id", "RegistrationNo");
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriver = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First();
            //ViewBag.DriverName = $"{loggedInDriver.FirstName} {loggedInDriver.LastName}";
            var fullName = $"{loggedInDriver.FirstName} {loggedInDriver.LastName}";
            ViewBag.DriverName = fullName;

            // Vehicle requests list

            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            var requestedVehicles = db.vehiclerequests.Where(vr => vr.status == ApplicationWideConstants.VehicleRequestOpen && vr.DriverId == loggedInDriverId).ToList();
            ViewBag.RequestedVehicles = requestedVehicles;

            // end vehicle requests list


            // hired vehicles start
            
            var currentlyHiredVehicles = db.HiredVehicles.Where(hv => hv.HireEndDate == null && hv.DriverId == loggedInDriverId).ToList();
            ViewBag.CurrentlyHiredVehicles = currentlyHiredVehicles;

            // hired vehicles end

            return View();
        }

        #region Issues

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
                model.status = ApplicationWideConstants.IssueStatusPending;
                model.reportdate = DateTime.Now;
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

        #endregion

        #region Notices

        public ActionResult GiveNotice()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            ViewBag.DriverId = loggedInDriverId;
            return View();
        }

        [HttpPost]
        public ActionResult GiveNotice(Notice model)
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            model.DriverId = loggedInDriverId;
            if (ModelState.IsValid)
            {
                db.Notices.Add(model);
                db.SaveChanges();
                ViewBag.NoticeId = model.Id;
                return View("NoticeTicket");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields correctly.");
                ViewBag.DriverId = loggedInDriverId;
                return View(model);
            }
        }

        public ActionResult NoticeList()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            var noticesRelatedToLoggedInDriver = db.Notices.Where(notice => notice.DriverId == loggedInDriverId).ToList();
            return View(noticesRelatedToLoggedInDriver);
        }

        public ActionResult NoticeDetails(int id)
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            Notice model = null;
            try
            {
                model = db.Notices.Where(notice => notice.DriverId == loggedInDriverId).Where(notice => notice.Id == id).First();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var replyexists = true;
            noticereply reply = null;

            try
            {
                reply = db.noticereplies.Where(nr => nr.noticeid == id).First();
            }
            catch (Exception ex)
            {
                replyexists = false;
            }
            ViewBag.ReplyExists = replyexists;
            if (replyexists)
            {
                ViewBag.Reply = reply;
            }

            return View(model);
        }

        #endregion

        #region VehicleRequests

        public ActionResult RequestVehicle()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles.Where(vehicle => vehicle.UnderOffer == null || vehicle.UnderOffer == false), "Id", "RegistrationNo");
            return View();
        }

        [HttpPost]
        public ActionResult RequestVehicle(vehiclerequest model)
        {
            if(ModelState.IsValid)
            {
                var loggedInUserId = User.Identity.GetUserId();
                var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
                model.DriverId = loggedInDriverId;
                model.RequestDate = DateTime.Now;
                model.status = ApplicationWideConstants.VehicleRequestOpen;
                db.vehiclerequests.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.VehicleId = new SelectList(db.Vehicles.Where(vehicle => vehicle.UnderOffer == null || vehicle.UnderOffer == false), "Id", "RegistrationNo", model.VehicleId);
                ModelState.AddModelError("", "Please fill all the fields correctly.");
                return View(model);
            }
        }

        public ActionResult VehicleRequestsList()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            var requestedVehicles = db.vehiclerequests.Where(vr => vr.status == ApplicationWideConstants.VehicleRequestOpen && vr.DriverId == loggedInDriverId).ToList();
            ViewBag.RequestedVehicles = requestedVehicles;
            return View();
        }

        #endregion

        #region HiredVehicles

        public ActionResult HiredVehiclesList()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInDriverId = db.Drivers.Where(d => d.UserId.Equals(loggedInUserId)).First().Id;
            var currentlyHiredVehicles = db.HiredVehicles.Where(hv => hv.HireEndDate == null && hv.DriverId == loggedInDriverId).ToList();
            ViewBag.CurrentlyHiredVehicles = currentlyHiredVehicles;
            return View();
        }

        #endregion
    }
}