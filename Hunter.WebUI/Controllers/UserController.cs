using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class UserController : Controller
    {
        public UserController(Managers.Manager manager)
        {
            this.Manager = manager;
        }

        public Managers.Manager Manager { get;set;}

        public IActionResult Login(Models.User.Login login)
        {
            var result = this.Manager.UserManager.Login(login);
            if (result is Models.DataResult<Models.ApplicationUser> applicationUser)
            {
                return this.Login(applicationUser.Data);
                return this.Ok(result);
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

        public IActionResult GetClaims()
        {
            return this.Json(this.User?.Claims);
        }

        public IActionResult List()
        {
            return this.View();
        }

        public IActionResult Query([FromBody]Models.PageParam<Models.User.Condition> pageParam)
        {
            var result = this.Manager.UserManager.Query(pageParam);
            return this.Json(result);
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
                return this.View(edit);
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody]Models.User.Edit edit)
        {
            var result = this.Manager.UserManager.Save(edit);
            if (result.Success)
                return this.Ok(result);
            return this.BadRequest(result);
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            var result = this.Manager.UserManager.Remove(id);
            if (result.Success)
                return this.Ok(result);
            return this.BadRequest(result);
        }

    }
}