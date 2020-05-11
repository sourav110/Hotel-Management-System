using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        AccomodationPackageService accomodationPackageService = new AccomodationPackageService();
        AccomodationTypeService accomodationTypeService = new AccomodationTypeService();
        // GET: Dashboard/AccomodationPackages
        public ActionResult Index(string searchTerm, int? accomodationTypeId, int? pageNo)
        {
            int recordSize = 5;
            pageNo = pageNo ?? 1;

            AccomodationPackagesListingViewModel model = new AccomodationPackagesListingViewModel();
            model.AccomodationTypeId = accomodationTypeId;

            model.AccomodationPackages = accomodationPackageService.SearchAccomodationPackages(searchTerm, accomodationTypeId, pageNo.Value, recordSize);
            model.AccomodationTypes = accomodationTypeService.GetAllAccomodationTypes();

            var totalRecords = accomodationPackageService.SearchAccomodationPackagesCount(searchTerm, accomodationTypeId);
            model.Pager = new Pager(totalRecords, pageNo, recordSize);

            return View(model);
        }


        [HttpGet]
        public ActionResult Action(int? id)
        {
            AccomodationPackagesActionViewModel model = new AccomodationPackagesActionViewModel();
            if (id.HasValue)
            {
                // edit
                var accomodationPackage = accomodationPackageService.GetAccomodationPackageById(id.Value);

                model.Id = accomodationPackage.Id;
                model.AccomodationTypeId = accomodationPackage.AccomodationTypeId;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
            }
            model.AccomodationTypes = accomodationTypeService.GetAllAccomodationTypes();
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
                var accomodationPackage = accomodationPackageService.GetAccomodationPackageById(model.Id);

                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = accomodationPackageService.UpdateAccomodationPackage(accomodationPackage);
            }
            else
            {
                // create
                AccomodationPackage accomodationPackage = new AccomodationPackage();

                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.Name = model.Name;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;

                result = accomodationPackageService.SaveAccomodationPackage(accomodationPackage);
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

            var accomodationPackage = accomodationPackageService.GetAccomodationPackageById(id);
            model.Id = accomodationPackage.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public JsonResult Delete(AccomodationPackagesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            var accomodationPackage = accomodationPackageService.GetAccomodationPackageById(model.Id);

            result = accomodationPackageService.DeleteAccomodationPackage(accomodationPackage);

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