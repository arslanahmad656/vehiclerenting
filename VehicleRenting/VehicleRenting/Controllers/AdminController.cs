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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private Entities db = new Entities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Contracts

        public ActionResult ContractIndex()
        {
            return View(db.Contracts.ToList());
        }

        public ActionResult CreateContract()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "RegistrationNo");
            return View();
        }

        [HttpPost]
        public ActionResult CreateContract(Contract model)
        {
            if(ModelState.IsValid)
            {
                var file = Request.Files[0];
                string fileName = null;
                string serverPath = null;
                string fullPath = null;
                string newPath = null;

                if (file != null && file.ContentLength > 0)
                {
                    //fileName = Path.GetFileName(file.FileName);
                    fileName = new Guid().ToString();
                    serverPath = Server.MapPath("~/Content/");
                    fullPath = Path.Combine(serverPath, fileName);
                    file.SaveAs(fullPath);
                }

                model.DocumentPath = fullPath;

                db.Contracts.Add(model);
                db.SaveChanges();

                if (file != null && file.ContentLength > 0)
                {
                    newPath = Path.Combine(serverPath, $"contract_{model.Id}{Path.GetExtension(fullPath)}");
                    System.IO.File.Move(fullPath, newPath);
                    model.DocumentPath = Path.GetFileName(newPath);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ContractIndex");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "RegistrationNo", model.VehicleId);
                return View(model);
            }
        }

        public ActionResult ContractDetails(int id)
        {
            var contract = db.Contracts.Find(id);
            if(contract == null)
            {
                return new HttpNotFoundResult("Contract not found with the specified id");
            }
            return View(contract);
        }

        public ActionResult DeleteContract(int id)
        {
            var contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return new HttpNotFoundResult("Contract not found with the specified id");
            }
            if(!string.IsNullOrWhiteSpace(contract.DocumentPath))
            {
                var serverPath = Server.MapPath("~/Content/");
                var fullPath = Path.Combine(serverPath, contract.DocumentPath);
                System.IO.File.Delete(fullPath);
            }
            db.Entry(contract).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ContractIndex");
        }

        public ActionResult EditContract(int id)
        {
            var model = db.Contracts.Find(id);
            if(model == null)
            {
                return new HttpNotFoundResult("No contract found with the specified id.");
            }
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "RegistrationNo", model.VehicleId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditContract(Contract model)
        {
            if(ModelState.IsValid)
            {
                var serverPath = Server.MapPath("~/Content/");
                var fileName = $"contract_{model.Id}";
                var completePath = Path.Combine(serverPath, fileName);
                var file = Request.Files[0];
                fileName = null;
                serverPath = null;
                string fullPath = null;

                if (file != null && file.ContentLength > 0)
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        System.IO.File.Delete(completePath);
                    }

                    fileName = $"contract_{model.Id}";
                    serverPath = Server.MapPath("~/Content/");
                    fullPath = Path.Combine(serverPath, fileName);
                    file.SaveAs(fullPath);

                    model.DocumentPath = fileName;
                }
                else
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        model.DocumentPath = $"contract_{model.Id}";
                    }
                    else
                    {
                        model.DocumentPath = "nofile";
                    }
                }


                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ContractIndex");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "RegistrationNo", model.VehicleId);
                return View(model);
            }
        }

        public FileResult DownloadContractDocument(string documentName)
        {
            var serverPath = Server.MapPath("~/Content/");
            var fullPath = Path.Combine(serverPath, documentName);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, documentName);
        }

        #endregion

        #region Proprietor

        public ActionResult ProprietorIndex()
        {
            return View(db.proprietors.ToList());
        }

        public ActionResult CreateProprietor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProprietor(proprietor model)
        {
            if(ModelState.IsValid)
            {
                db.proprietors.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Plaease fill in all the fields properly.");
                return View(model);
            }
        }

        public ActionResult EditProprietor(int id)
        {
            var model = db.proprietors.Find(id);
            if(model == null)
            {
                return new HttpNotFoundResult("No proprietor found with the specified ID.");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProprietor(proprietor model)
        {
            if(ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProprietorIndex");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                return View(model);
            }
        }

        public ActionResult ProprietorDetails(int id)
        {
            var model = db.proprietors.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("No proprietor found with the specified ID.");
            }
            return View(model);
        }

        public ActionResult DeleteProprietor(int id)
        {
            var model = db.proprietors.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("No proprietor found with the specified ID.");
            }
            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ProprietorIndex");
        }

        #endregion

        #region Vehicles

        public ActionResult VehicleIndex()
        {
            return View(db.Vehicles.AsEnumerable());
        }

        public ActionResult CreateVehicle()
        {
            ViewBag.ProprietorId = new SelectList(db.proprietors, "Id", "Name");
            ViewBag.VehicleConditionId = new SelectList(db.VehicleConditions, "Id", "Title");
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult CreateVehicle(Vehicle model)
        {
            if(ModelState.IsValid)
            {
                db.Vehicles.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProprietorId = new SelectList(db.proprietors, "Id", "Name", model.ProprietorId);
                ViewBag.VehicleConditionId = new SelectList(db.VehicleConditions, "Id", "Title", model.VehicleConditionId);
                ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Title", model.VehicleTypeId);
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                return View(model);
            }
        }

        public ActionResult EditVehicle(int id)
        {
            var model = db.Vehicles.Find(id);
            if(model == null)
            {
                return new HttpNotFoundResult("No vehicle found with the specified ID.");
            }
            ViewBag.ProprietorId = new SelectList(db.proprietors, "Id", "Name", model.ProprietorId);
            ViewBag.VehicleConditionId = new SelectList(db.VehicleConditions, "Id", "Title", model.VehicleConditionId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Title", model.VehicleTypeId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVehicle(Vehicle model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VehicleIndex");
            }
            else
            {
                ViewBag.ProprietorId = new SelectList(db.proprietors, "Id", "Name", model.ProprietorId);
                ViewBag.VehicleConditionId = new SelectList(db.VehicleConditions, "Id", "Title", model.VehicleConditionId);
                ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Title", model.VehicleTypeId);
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                return View(model);
            }
        }

        public ActionResult VehicleDetails(int id)
        {
            var model = db.Vehicles.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("No vehicle found with the specified ID.");
            }
            ViewBag.ProprietorId = new SelectList(db.proprietors, "Id", "Name", model.ProprietorId);
            ViewBag.VehicleConditionId = new SelectList(db.VehicleConditions, "Id", "Title", model.VehicleConditionId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Title", model.VehicleTypeId);
            return View(model);
        }

        public ActionResult DeleteVehicle(int id)
        {
            var model = db.Vehicles.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("No vehicle found with the specified ID.");
            }
            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("VehicleIndex");
        }

        #endregion

        #region Driver

        public ActionResult DriverIndex()
        {
            return View(db.Drivers.AsEnumerable());
        }

        public ActionResult DriverDetails(int id)
        {
            var model = db.Drivers.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("No vehicle found with the specified ID.");
            }
            ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title", model.SalutationId);
            ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title", model.ReferenceId);
            ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title", model.IdentityId);
            ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title", model.NationalityId);
            ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title", model.SourceId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Username", model.UserId);
            return View(model);
        }

        public ActionResult CreateDriver()
        {
            ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title");
            ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title");
            ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title");
            ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title");
            ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult CreateDriver(DriverUserRegisterViewModel model)
        {
            ApplicationUser user = null;
            bool userCreated = false;
            bool userAddedToRole = false;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.Password.Equals(model.ConfirmPassword))
                    {
                        throw new Exception("Password and Confirm password fields should have the same value.");
                    }

                    user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        PhoneNumber = model.Phone
                    };

                    var status = UserManager.Create(user, model.Password);

                    if(!status.Succeeded)
                    {
                        var errors = "User could not be created.<br/>";
                        foreach (var error in status.Errors)
                        {
                            errors += error + "<br/>";
                        }
                        throw new Exception(errors);
                    }
                    userCreated = true;

                    status = UserManager.AddToRole(user.Id, ApplicationWideConstants.DriverRoleName);

                    if(!status.Succeeded)
                    {
                        var errors = "User could not be added to the driver role.<br/>";
                        foreach (var error in status.Errors)
                        {
                            errors += error + "<br/>";
                        }
                        throw new Exception(errors);
                    }
                    userAddedToRole = true;

                    var referenceDocument = Request.Files["ReferenceDocumentPath"];
                    if ((model.ReferenceId != null && (referenceDocument == null || referenceDocument.ContentLength == 0))
                        || (model.ReferenceId == null && (referenceDocument != null || referenceDocument.ContentLength > 0)))
                    {
                        var error = "Either you have selected a reference type and not supplied reference document or you have not selected a reference type and supplied a reference document. Please correct this anomoly by either supplying both or none.";
                        throw new Exception(error);
                    }

                    var identityDocument = Request.Files["IdentityDocuementPath"];
                    if ((model.IdentityId != null && (identityDocument == null || identityDocument.ContentLength == 0))
                        || (model.IdentityId == null && (identityDocument != null || identityDocument.ContentLength > 0)))
                    {
                        var error = "Either you have selected an identity type and not supplied identity document or you have not selected an identity type and supplied an identity document. Please correct this anomoly by either supplying both or none.";
                        throw new Exception(error);
                    }

                    var driver = new Driver
                    {
                        AdminFee = model.AdminFee,
                        AdvancedRentAmount = model.AdvancedRentAmount,
                        ContractFrom = model.ContractFrom,
                        ContractLength = model.ContractLength,
                        ContractTo = model.ContractTo,
                        FirstName = model.FirstName,
                        HoldingDepositAmount = model.HoldingDepositAmount,
                        IdentityDocuementPath = model.IdentityDocuementPath,
                        IdentityId = model.IdentityId,
                        LastName = model.LastName,
                        NationalityId = model.NationalityId,
                        ReferenceDocumentPath = model.ReferenceDocumentPath,
                        ReferenceId = model.ReferenceId,
                        RentDate = model.RentDate,
                        RentDueDate = model.RentDueDate,
                        SalutationId = model.SalutationId,
                        SecurityDepositAmount = model.SecurityDepositAmount,
                        SourceId = model.SourceId,
                        SpecialConditions = model.SpecialConditions,
                        UserId = user.Id
                    };

                    db.Drivers.Add(driver);
                    db.SaveChanges();

                    bool modificationNeeded = false;
                    if(referenceDocument != null && referenceDocument.ContentLength > 0)
                    {
                        var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                        var fileName = $"{ApplicationWideConstants.DriverReferenceDocumentName}_{driver.Id}{Path.GetExtension(referenceDocument.FileName)}";
                        var fullPath = Path.Combine(serverPath, fileName);
                        referenceDocument.SaveAs(fullPath);
                        driver.ReferenceDocumentPath = fileName;
                        modificationNeeded = true;
                    }

                    if (identityDocument != null && identityDocument.ContentLength > 0)
                    {
                        var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                        var fileName = $"{ApplicationWideConstants.DriverIdentityTypeDocumentName}_{driver.Id}{Path.GetExtension(identityDocument.FileName)}";
                        var fullPath = Path.Combine(serverPath, fileName);
                        referenceDocument.SaveAs(fullPath);
                        driver.IdentityDocuementPath = fileName;
                        modificationNeeded = true;
                    }

                    if(modificationNeeded)
                    {
                        db.Entry(driver).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    if(userCreated)
                    {
                        IdentityResult result;
                        if(userAddedToRole)
                        {
                            result = UserManager.RemoveFromRole(user.Id, ApplicationWideConstants.DriverRoleName);
                        }
                        result = UserManager.Delete(user);
                    }

                    ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title", model.SalutationId);
                    ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title", model.ReferenceId);
                    ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title", model.IdentityId);
                    ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title", model.NationalityId);
                    ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title", model.SourceId);

                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title", model.SalutationId);
                ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title", model.ReferenceId);
                ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title", model.IdentityId);
                ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title", model.NationalityId);
                ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title", model.SourceId);
                ModelState.AddModelError("", "Model state is invalid. Please fill in all fields properly.");
                return View(model);
            }
        }

        public ActionResult EditDriver(int id)
        {
            var model = db.Drivers.Find(id);
            if(model == null)
            {
                return new HttpNotFoundResult("Could not find the driver with the specified id.");
            }
            ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title", model.SalutationId);
            ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title", model.ReferenceId);
            ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title", model.IdentityId);
            ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title", model.NationalityId);
            ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title", model.SourceId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Username", model.UserId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditDriver(Driver model)
        {
            if(ModelState.IsValid)
            {
                var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                var referenceDocument = Request.Form["reference_old"];
                var completePath = Path.Combine(serverPath, referenceDocument);
                var file = Request.Files["ReferenceDocumentPath"];
                referenceDocument = null;
                serverPath = null;
                string fullPath = null;

                if (file != null && file.ContentLength > 0)
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        System.IO.File.Delete(completePath);
                    }

                    referenceDocument = $"{ApplicationWideConstants.DriverReferenceDocumentName}_{model.Id}{Path.GetExtension(file.FileName)}";
                    serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                    fullPath = Path.Combine(serverPath, referenceDocument);
                    file.SaveAs(fullPath);

                    model.ReferenceDocumentPath = referenceDocument;
                }
                else
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        model.ReferenceDocumentPath = Request.Form["reference_old"];
                    }
                    else
                    {
                        model.ReferenceDocumentPath = "nofile";
                    }
                }

                /////

                serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                var identityDocument = Request.Form["identity_old"];
                completePath = Path.Combine(serverPath, identityDocument);
                file = Request.Files["IdentityDocuementPath"];
                identityDocument = null;
                serverPath = null;
                fullPath = null;

                if (file != null && file.ContentLength > 0)
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        System.IO.File.Delete(completePath);
                    }

                    identityDocument = $"{ApplicationWideConstants.DriverIdentityTypeDocumentName}_{model.Id}{Path.GetExtension(file.FileName)}";
                    serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                    fullPath = Path.Combine(serverPath, identityDocument);
                    file.SaveAs(fullPath);

                    model.IdentityDocuementPath = identityDocument;
                }
                else
                {
                    if (System.IO.File.Exists(completePath))
                    {
                        model.IdentityDocuementPath = Request.Form["identity_old"];
                    }
                    else
                    {
                        model.IdentityDocuementPath = "nofile";
                    }
                }

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverIndex");

            }
            else
            {
                ViewBag.SalutationId = new SelectList(db.Salutations, "Id", "Title", model.SalutationId);
                ViewBag.ReferenceId = new SelectList(db.ReferenceTypes, "Id", "Title", model.ReferenceId);
                ViewBag.IdentityId = new SelectList(db.IdentityTypes, "Id", "Title", model.IdentityId);
                ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "Title", model.NationalityId);
                ViewBag.SourceId = new SelectList(db.SourceTypes, "Id", "Title", model.SourceId);
                ModelState.AddModelError("", "Model state is invalid. Please fill in all fields properly.");
                return View(model);
            }
        }

        public ActionResult DeleteDriver(int id)
        {
            var model = db.Drivers.Find(id);
            if (model == null)
            {
                return new HttpNotFoundResult("Contract not found with the specified id");
            }
            if (!string.IsNullOrWhiteSpace(model.ReferenceDocumentPath) && !model.ReferenceDocumentPath.Equals("nofile", StringComparison.OrdinalIgnoreCase))
            {
                var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                var fullPath = Path.Combine(serverPath, model.ReferenceDocumentPath);
                System.IO.File.Delete(fullPath);
            }
            if (!string.IsNullOrWhiteSpace(model.IdentityDocuementPath) && !model.IdentityDocuementPath.Equals("nofile", StringComparison.OrdinalIgnoreCase))
            {
                var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
                var fullPath = Path.Combine(serverPath, model.IdentityDocuementPath);
                System.IO.File.Delete(fullPath);
            }

            var userId = model.UserId;

            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            
            IdentityResult result;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            result = UserManager.RemoveFromRole(userId, ApplicationWideConstants.DriverRoleName);
            result = UserManager.Delete(context.Users.Find(userId));

            return RedirectToAction("DriverIndex");
        }

        public FileResult DownloadReferenceDocument(string documentName)
        {
            var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
            var fullPath = Path.Combine(serverPath, documentName);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, documentName);
        }

        public FileResult DownloadIdentityDocument(string documentName)
        {
            var serverPath = Server.MapPath(ApplicationWideConstants.RootStoragePath);
            var fullPath = Path.Combine(serverPath, documentName);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, documentName);
        }

        #endregion

        #region Issues

        public ActionResult ReportedIssuesList()
        {
            return View(db.Issues.ToList());
        }

        public ActionResult IssueDetails(int id)
        {
            var model = db.Issues.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No issue found with the specified ticket.");
            }
            return View(model);
        }

        public ActionResult CloseIssue(int id)
        {
            var model = db.Issues.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No issue found with the specified ticket.");
            }
            model.status = ApplicationWideConstants.IssueStatusClosed;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ReportedIssuesList");
        }

        #endregion

        #region Notice

        public ActionResult NoticeList()
        {
            return View(db.Notices.ToList());
        }

        public ActionResult NoticeDetails(int id)
        {
            Notice model = db.Notices.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No notice found with the specified ticket.");
            }
            var replyexists = true;
            noticereply reply = null;
            try
            {
                reply = db.noticereplies.Where(nr => nr.noticeid == id).First();
            }
            catch(Exception ex)
            {
                replyexists = false;
            }
            ViewBag.ReplyExists = replyexists;
            if(replyexists)
            {
                ViewBag.Reply = reply;
            }
            return View(model);
        }

        public ActionResult ReplyToNotice(int id)   // notice id
        {
            Notice model = db.Notices.First();
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No notice found with the specified ticket.");
            }
            ViewBag.NoticeId = id;
            return View();
        }

        [HttpPost]
        public ActionResult ReplyToNotice(noticereply model)
        {
            if (ModelState.IsValid)
            {
                model.replydate = DateTime.Now;
                db.noticereplies.Add(model);
                db.SaveChanges();
                return RedirectToAction("NoticeList");
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all the fields properly.");
                return View(model);
            }
        }

        #endregion

        #region VehicleRequests

        public ActionResult VehicleRequestsList()
        {
            var openVehicleRequests = db.vehiclerequests.Where(vr => vr.status == ApplicationWideConstants.VehicleRequestOpen).ToList();
            ViewBag.RequestedVehicles = openVehicleRequests;
            return View();
        }

        public ActionResult ApproveVehicleRequest(int id) // vehicle request id
        {
            var model = db.vehiclerequests.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.status = ApplicationWideConstants.VehicleRequestClosed;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            var hiredVehicle = model.Vehicle;
            hiredVehicle.UnderOffer = ApplicationWideConstants.VehicleHired;
            db.Entry(hiredVehicle).State = EntityState.Modified;
            db.SaveChanges();

            CloseVehicleRequestsBasedOnVehicleId(model.VehicleId);

            var hiredVehicleEntry = new HiredVehicle
            {
                DriverId = model.DriverId,
                HireStartDate = DateTime.Now,
                VehicleId = model.VehicleId,
                HireEndDate = null
            };
            db.HiredVehicles.Add(hiredVehicleEntry);
            db.SaveChanges();

            return RedirectToAction("VehicleRequestsList");
        }

        public ActionResult DeclineVehicleRequest(int id)
        {
            var model = db.vehiclerequests.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.status = ApplicationWideConstants.VehicleRequestClosed;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("VehicleRequestsList");
        }

        private void CloseVehicleRequestsBasedOnVehicleId(int vehicleId)
        {
            var openRequestsWithGivenVehicleId = db.vehiclerequests.Where(vr => vr.status == ApplicationWideConstants.VehicleRequestOpen && vr.VehicleId == vehicleId).ToList();
            openRequestsWithGivenVehicleId.ForEach(vr =>
            {
                vr.status = ApplicationWideConstants.VehicleRequestClosed;
                db.Entry(vr).State = EntityState.Modified;
            });
            db.SaveChanges();
        }

        private void CloseVehicleRequestsBasedOnDriverId(int driverId)
        {
            var openRequestsWithGivenDriverId = db.vehiclerequests.Where(vr => vr.status == ApplicationWideConstants.VehicleRequestOpen && vr.DriverId == driverId).ToList();
            openRequestsWithGivenDriverId.ForEach(vr =>
            {
                vr.status = ApplicationWideConstants.VehicleRequestClosed;
                db.Entry(vr).State = EntityState.Modified;
            });
            db.SaveChanges();
        }

        #endregion

        #region HiredVehicles

        public ActionResult HiredVehiclesList()
        {
            var currentlyHiredVehicles = db.HiredVehicles.Where(hv => hv.HireEndDate == null).ToList();
            ViewBag.CurrentlyHiredVehicles = currentlyHiredVehicles;
            return View();
        }

        public ActionResult RevokeVehicle(int id) // hired vehicle id
        {
            var model = db.HiredVehicles.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            model.HireEndDate = DateTime.Now;
            model.Vehicle.UnderOffer = ApplicationWideConstants.VehicleAvailable;
            db.Entry(model.Vehicle).State = EntityState.Modified;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("HiredVehiclesList");
        }

        #endregion

        #region others

        private ActionResult GetAllFiles()
        {
            var serverPath = Server.MapPath("~/Content/");
            var files = Directory.GetFiles(serverPath);
            ViewBag.Files = files;
            return View();
        }

        #endregion
    }
}