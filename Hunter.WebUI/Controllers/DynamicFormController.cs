using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class DynamicFormController : Controller
    {
        public DynamicFormController(IHostingEnvironment hostingEnvironment, Managers.Manager manager)
        {
            this.HostingEnvironment = hostingEnvironment;
            this.Manager = manager;
        }

        protected IHostingEnvironment HostingEnvironment { get; set; }

        protected Managers.Manager Manager { get; set; }

        public IActionResult List(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            return View(entity);
        }

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
        public IActionResult SaveData(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            this.Manager.DynamicFormManager.SaveData(id, dataID, dictionary);
            return this.Ok();
        }

        public IActionResult Find(string id, string dataID)
        {
            var data = this.Manager.DynamicFormManager.Find(id, dataID);
            return this.Json(data);
        }

        public IActionResult Remove(string id, string dataID)
        {
            this.Manager.DynamicFormManager.Remove(id, dataID);
            return this.Ok();
        }

        /*
        public IActionResult Download(string id, string dataID, string type)
        {
            var entity = this.Manager.DynamicFormManager.Find(id, dataID);
            //var html = this.Manager.DynamicFormManager.GetCompleteHtml(entity.Html, this.HostingEnvironment.WebRootPath);
            var html = this.Manager.DynamicFormManager.GetCompleteHtml(entity.Html, @"C:\Users\Administrator\Source\Repos\Hunter\Hunter.WebUI\wwwroot");
            var stream = this.Manager.DynamicFormManager.ParseHTML(html);
            return this.File(stream, "application/pdf");
        }
        */
    }
}