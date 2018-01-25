using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class DynamicFormController : Controller
    {
        public DynamicFormController(Managers.Manager manager)
        {
            this.Manager = manager;
        }

        protected Managers.Manager Manager { get; set; }

        [Route("DynamicForm/List/{id}")]
        public IActionResult List(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            return View(entity);
        }

        [Route("DynamicForm/Query/{id}")]
        public IActionResult Query(string id, [FromBody]Models.PageParam<Models.DynamicForm.Condition> pageParam)
        {
            var result = this.Manager.DynamicFormManager.Query(id, pageParam);
            return this.Json(result);
        }

        [HttpGet]
        public IActionResult Edit(string id, string dataID)
        {
            var entity = this.Manager.FormManager.Find(id);
            var data = this.Manager.DynamicFormManager.Find(id, dataID);
            if (data == null)
                dataID = this.Manager.DynamicFormManager.GenerateMongoID;
            this.ViewData["id"] = id;
            this.ViewData["DataID"] = dataID;
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult Save(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            this.Manager.DynamicFormManager.Save(id, dataID, dictionary);
            return this.Ok();
        }

        public IActionResult GetData(string id, string dataID)
        {
            var data = this.Manager.DynamicFormManager.Find(id, dataID);
            return this.Json(data?.ToDictionary());
        }

        public IActionResult Remove(string id, string dataID)
        {
            this.Manager.DynamicFormManager.Remove(id, dataID);
            return this.Ok();
        }
    }
}