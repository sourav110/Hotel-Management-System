using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Controllers
{
    public class AccomodationsController : Controller
    {
        // GET: Accomodations
        public ActionResult Index(int? accomodationTypeId)
        {
            return View();
        }
    }
}