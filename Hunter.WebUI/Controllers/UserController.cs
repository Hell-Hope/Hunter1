using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{

    public class UserController : SharedController
    {
        public UserController(Managers.Manager manager) : base(manager)
        {
            this.Manager = manager;
        }

        public IActionResult List()
        {
            return this.View();
        }

        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Query([FromBody]Models.PageParam<Models.User.Condition> pageParam)
        {
            var result = this.Manager.UserManager.Query(pageParam);
            return this.ActionResult(result);
        }

        public IActionResult Edit(string id)
        {
            var edit = this.Manager.UserManager.GetEdit(id) ?? new Models.User.Edit();
            if (String.IsNullOrWhiteSpace(edit.ID))
                edit.ID = this.Manager.FormManager.GenerateMongoID;
            if (String.Equals(this.Request.Method, "post", StringComparison.OrdinalIgnoreCase))
            {
                return this.Ok(edit);
            }
            else
            {
                this.ModelState.Clear();
                this.ViewData["Permits"] = this.Manager.PermitManager.GetAllForChoose();
                return this.View(edit);
            }
        }

        [HttpPost]
        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Save([FromBody]Models.User.Edit edit)
        {
            var result = this.Manager.UserManager.Save(edit);
            return this.ActionResult(result);
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            var result = this.Manager.UserManager.Remove(id);
            return this.ActionResult(result);
        }

        public IActionResult Logout()
        {
            return this.SignOut(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Login(Models.User.Login login)
        {
            var result = this.Manager.UserManager.Login(login);
            if (result is Models.DataResult<Models.ApplicationUser> applicationUser)
            {
                return this.Login(applicationUser.Data);
            }
            return this.BadRequest(result);
        }

        [NonAction]
        public ActionResult Login(Models.ApplicationUser applicationUser)
        {
            var claims = new System.Security.Claims.Claim[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, applicationUser.ID),
                new System.Security.Claims.Claim(nameof(applicationUser.ID), applicationUser.ID),
                new System.Security.Claims.Claim(nameof(applicationUser.Account), applicationUser.Account),
                new System.Security.Claims.Claim(nameof(applicationUser.Name), applicationUser.Name)
            };
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(claims, "Basic");
            var claimsPrincipal = new System.Security.Claims.ClaimsPrincipal(claimsIdentity);
            var result = this.SignIn(claimsPrincipal, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return result;
        }
        

    }
}