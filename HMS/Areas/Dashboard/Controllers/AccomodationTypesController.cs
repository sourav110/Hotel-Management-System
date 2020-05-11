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
    public class AccomodationTypesController : Controller
    {
        AccomodationTypeService accomodationTypeService = new AccomodationTypeService();

        // GET: Dashboard/AccomodationTypes
        public ActionResult Index(string searchTerm)
        {
            AccomodationTypesListingViewModel model = new AccomodationTypesListingViewModel();
            model.AccomodationTypes = accomodationTypeService.SearchAccomodationTypes(searchTerm);

            return View(model);
        }

        //public ActionResult Listing()
        //{
        //    AccomodationTypesListingViewModel model = new AccomodationTypesListingViewModel();
        //    model.AccomodationTypes = accomodationTypeService.GetAllAccomodationTypes();

        //    return PartialView("_Listing", model);
        //}

        [HttpGet]
        public ActionResult Action(int? id)
        {
            AccomodationTypesActionViewModel model = new AccomodationTypesActionViewModel();
            if (id.HasValue)
            {
                // edit
                var accomodationType = accomodationTypeService.GetAccomodationTypeById(id.Value);
                model.Id = accomodationType.Id;
                model.Name = accomodationType.Name;
                model.Description = accomodationType.Description;
            }
            return PartialView("_Action", model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationTypesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            if(model.Id > 0)
            {
                // edit
                var accomodationType = accomodationTypeService.GetAccomodationTypeById(model.Id);
                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;

                result = accomodationTypeService.UpdateAccomodationType(accomodationType);
            }
            else
            {
                // create
                AccomodationType accomodationType = new AccomodationType();

                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;

                result = accomodationTypeService.SaveAccomodationType(accomodationType);
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
            AccomodationTypesActionViewModel model = new AccomodationTypesActionViewModel();
            
            var accomodationType = accomodationTypeService.GetAccomodationTypeById(id);
            model.Id = accomodationType.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public JsonResult Delete(AccomodationTypesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            var accomodationType = accomodationTypeService.GetAccomodationTypeById(model.Id);
            
            result = accomodationTypeService.DeleteAccomodationType(accomodationType);

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