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
                return this.Ok(edit);
            else
                return this.View(edit);
        }

        public IActionResult Save([FromBody]Models.User.Edit edit)
        {
            this.Manager.UserManager.Save(edit);
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            this.Manager.UserManager.Remove(id);
            return this.Ok();
        }

    }
}