using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller 
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRoleManager _roleManager;

        public RolesController()
        {
        }

        public RolesController(HMSUserManager userManager, HMSSignInManager signInManager, HMSRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public HMSSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<HMSSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public HMSUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HMSUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public HMSRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<HMSRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }



        // GET: Dashboard/Users
        public ActionResult Index(string searchTerm, int? pageNo)
        {
            int recordSize = 5;
            pageNo = pageNo ?? 1;

            RolesListingViewModel model = new RolesListingViewModel();
            model.SearchTerm = searchTerm;
            
            model.Roles = SearchRoles(searchTerm, pageNo.Value, recordSize);

            var totalRecords = SearchRolesCount(searchTerm);
            model.Pager = new Pager(totalRecords, pageNo, recordSize);

            return View(model);
        }

        public IEnumerable<IdentityRole> SearchRoles(string searchTerm, int pageNo, int recordSize)
        {
            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var skip = (pageNo - 1) * recordSize;

            return roles.OrderBy(x => x.Name).Skip(skip).Take(recordSize).ToList();
        }
         
        public int SearchRolesCount(string searchTerm)
        {
            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return roles.Count();
        }


        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {
            RolesActionViewModel model = new RolesActionViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                // edit
                var role = await RoleManager.FindByIdAsync(id);
                model.Id = role.Id;
                model.Name = role.Name;
            }

            return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<ActionResult> Action(RolesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.Id))
            {
                // edit
                var role = await RoleManager.FindByIdAsync(model.Id);
                role.Name = model.Name;

                result = await RoleManager.UpdateAsync(role);
            }
            else
            {
                // create
                var role = new IdentityRole();
                role.Name = model.Name;

                result = await RoleManager.CreateAsync(role);
            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(" ", result.Errors) };

            return json;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            RolesActionViewModel model = new RolesActionViewModel();

            var role = await RoleManager.FindByIdAsync(id);
            model.Id = role.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(RolesActionViewModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.Id))
            {
                // edit
                var role = await RoleManager.FindByIdAsync(model.Id);

                result = await RoleManager.DeleteAsync(role);

                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invaild Role" };
            }

            return json;
        }
    }
}