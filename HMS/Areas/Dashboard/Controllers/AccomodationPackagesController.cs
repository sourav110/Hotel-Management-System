using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        AccomodationPackageService AccomodationPackageService = new AccomodationPackageService();
        // GET: Dashboard/AccomodationPackages
        public ActionResult Index(string searchTerm)
        {
            AccomodationPackagesListingViewModel model = new AccomodationPackagesListingViewModel();
            model.AccomodationPackages = AccomodationPackageService.SearchAccomodationPackages(searchTerm);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? id)
        {
            AccomodationPackagesActionViewModel model = new AccomodationPackagesActionViewModel();
            if (id.HasValue)
            {
                // edit
                var accomodationPackage = AccomodationPackageService.GetAccomodationPackageById(id.Value);
                model.Id = accomodationPackage.Id;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
            }
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationPackagesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            if (model.Id > 0)
            {
                // edit
                var accomodationPackage = AccomodationPackageService.GetAccomodationPackageById(model.Id);
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = AccomodationPackageService.UpdateAccomodationPackage(accomodationPackage);
            }
            else
            {
                // create
                AccomodationPackage accomodationPackage = new AccomodationPackage();

                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = AccomodationPackageService.SaveAccomodationPackage(accomodationPackage);
            }

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to Save" };
            }

            return json;
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            AccomodationPackagesActionViewModel model = new AccomodationPackagesActionViewModel();

            var accomodationPackage = AccomodationPackageService.GetAccomodationPackageById(id);
            model.Id = accomodationPackage.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public JsonResult Delete(AccomodationPackagesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            var accomodationPackage = AccomodationPackageService.GetAccomodationPackageById(model.Id);

            result = AccomodationPackageService.DeleteAccomodationPackage(accomodationPackage);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to Delete" };
            }

            return json;
        }
    }
}