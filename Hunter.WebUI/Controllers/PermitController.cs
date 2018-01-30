using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class PermitController : Controller
    {
        public PermitController(Managers.Manager manager)
        {
            this.Manager = manager;
        }

        public Managers.Manager Manager { get; set; }

        public IActionResult List()
        {
            return this.View();
        }

        public IActionResult Query([FromBody]Models.PageParam<Models.Permit.Condition> pageParam)
        {
            var result = this.Manager.PermitManager.Query(pageParam);
            return this.Json(result);
        }

        public IActionResult Edit(string id)
        {
            var edit = this.Manager.PermitManager.GetEdit(id) ?? new Models.Permit.Edit();
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

        public IActionResult Save([FromBody]Models.Permit.Edit edit)
        {
            this.Manager.PermitManager.Save(edit);
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            this.Manager.PermitManager.Remove(id);
            return this.Ok();
        }

    }
}