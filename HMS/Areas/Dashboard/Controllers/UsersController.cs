using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class UsersController : Controller
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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



        // GET: Dashboard/Users
        public ActionResult Index(string searchTerm, string roleId, int? pageNo)
        {
            int recordSize = 5; 
            pageNo = pageNo ?? 1;

            UsersListingViewModel model = new UsersListingViewModel();
            model.SearchTerm = searchTerm;
            model.RoleId = roleId;


            //model.Roles = accomodationService.SearchAccomodations(searchTerm, accomodationPackageId, pageNo.Value, recordSize);
            model.Users = SearchUsers(searchTerm, roleId, pageNo.Value, recordSize);

            var totalRecords = SearchUsersCount(searchTerm, roleId);
            model.Pager = new Pager(totalRecords, pageNo, recordSize);

            return View(model);
        }

        public IEnumerable<HMSUser> SearchUsers(string searchTerm, string roleId, int pageNo, int recordSize)
        {
            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                //users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            var skip = (pageNo - 1) * recordSize;

            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchUsersCount(string searchTerm, string roleId)
        {
            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                //users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            return users.Count();
        }


        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {
            UsersActionViewModel model = new UsersActionViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                // edit
                var user = await UserManager.FindByIdAsync(id);

                model.Id = user.Id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.Country = user.Country;
                model.City = user.City;
                model.Address = user.Address;
                
            }
            //model.AccomodationPackages = accomodationPackageService.GetAllAccomodationPackages();
            return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<ActionResult> Action(UsersActionViewModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.Id))
            {
                // edit
                var user = await UserManager.FindByIdAsync(model.Id);

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Country = model.Country;
                user.City = model.City;
                user.Address = model.Address;

                result = await UserManager.UpdateAsync(user);
            }
            else
            {
                // create
                var user = new HMSUser();

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Country = model.Country;
                user.City = model.City;
                user.Address = model.Address;

                result = await UserManager.CreateAsync(user);
            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(" ", result.Errors) };

            return json;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            UsersActionViewModel model = new UsersActionViewModel();

            var user = await UserManager.FindByIdAsync(id);
            model.Id = user.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(UsersActionViewModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.Id))
            {
                // edit
                var user = await UserManager.FindByIdAsync(model.Id);

                result = await UserManager.DeleteAsync(user);
                
                json.Data = new { Success = result.Succeeded, Message = string.Join(" ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invaild User" };
            }

            return json;
        }
    }
}