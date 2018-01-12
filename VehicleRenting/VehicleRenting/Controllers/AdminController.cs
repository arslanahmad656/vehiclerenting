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
                string imageName = null;
                string serverPath = null;
                string fullPath = null;
                string newPath = null;

                if (file != null && file.ContentLength > 0)
                {
                    imageName = Path.GetFileName(file.FileName);
                    serverPath = Server.MapPath("~/Content/Files/Contracts");
                    fullPath = Path.Combine(serverPath, imageName);
                    file.SaveAs(fullPath);
                }

                model.DocumentPath = fullPath;

                db.Contracts.Add(model);
                db.SaveChanges();

                if (file != null && file.ContentLength > 0)
                {
                    newPath = Path.Combine(serverPath, $"{model.Id}{Path.GetExtension(fullPath)}");
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
                var serverPath = Server.MapPath("~/Content/Files/Contracts/");
                var fullPath = Path.Combine(serverPath, contract.DocumentPath);
                System.IO.File.Delete(fullPath);
            }
            db.Entry(contract).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ContractIndex");
        }

        public FileResult DownloadContractDocument(string documentName)
        {
            var serverPath = Server.MapPath("~/Content/Files/Contracts/");
            var fullPath = Path.Combine(serverPath, documentName);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, "Contract");
        }

        #endregion
    }
}