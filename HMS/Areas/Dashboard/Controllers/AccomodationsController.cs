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
    public class AccomodationsController : Controller
    {
        AccomodationService accomodationService = new AccomodationService();
        AccomodationPackageService accomodationPackageService = new AccomodationPackageService();

        // GET: Dashboard/Accomodations
        public ActionResult Index(string searchTerm, int? accomodationPackageId, int? pageNo)
        {
            int recordSize = 5;
            pageNo = pageNo ?? 1;

            AccomodationsListingViewModel model = new AccomodationsListingViewModel();
            model.AccomodationPackageId = accomodationPackageId;

            model.Accomodations = accomodationService.SearchAccomodations(searchTerm, accomodationPackageId, pageNo.Value, recordSize);
            model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackages();

            var totalRecords = accomodationService.SearchAccomodationsCount(searchTerm, accomodationPackageId);
            model.Pager = new Pager(totalRecords, pageNo, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? id)
        {
            AccomodationsActionViewModel model = new AccomodationsActionViewModel();
            if (id.HasValue)
            {
                // edit
                var accomodation = accomodationService.GetAccomodationById(id.Value);

                model.Id = accomodation.Id;
                model.AccomodationPackageId = accomodation.AccomodationPackageId;
                model.Name = accomodation.Name;
                model.Description = accomodation.Description;
            }
            model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackages();
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationsActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            if (model.Id > 0)
            {
                // edit
                var accomodation = accomodationService.GetAccomodationById(model.Id);

                accomodation.AccomodationPackageId = model.AccomodationPackageId;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;

                result = accomodationService.UpdateAccomodation(accomodation);
            }
            else
            {
                // create
                Accomodation accomodation = new Accomodation();

                accomodation.AccomodationPackageId = model.AccomodationPackageId;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;

                result = accomodationService.SaveAccomodation(accomodation);
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
            AccomodationsActionViewModel model = new AccomodationsActionViewModel();

            var accomodation = accomodationService.GetAccomodationById(id);
            model.Id = accomodation.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public JsonResult Delete(AccomodationsActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            var accomodation = accomodationService.GetAccomodationById(model.Id);

            result = accomodationService.DeleteAccomodation(accomodation);

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