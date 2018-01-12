using System;
using System.IO;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using VehicleRenting.Models;

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

        #region others

        public ActionResult GetAllFiles()
        {
            var serverPath = Server.MapPath("~/Content/");
            var files = Directory.GetFiles(serverPath);
            ViewBag.Files = files;
            return View();
        }

        #endregion
    }
}