using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Hunter.Managers;

namespace Hunter.WebUI.Controllers
{
    public class FormController : Controller
    {
        public FormController(FormManager formManager, DynamicFormManager dynamicFormManager)
        {
            this.FormManager = formManager;
            this.DynamicFormManager = dynamicFormManager;
        }
        
        protected FormManager FormManager { get; set; }

        protected DynamicFormManager DynamicFormManager { get; set; }

        public IActionResult List()
        {
            return this.View();
        }

        public IActionResult Edit(string id)
        {
            var edit = this.FormManager.GetEdit(id) ?? new Models.Form.Edit() { ID = this.FormManager.GenerateMongoID };
            return this.View(edit);
        }

        public IActionResult Save([FromBody]Models.Form.Edit edit)
        {
            this.FormManager.Save(edit);
            return this.Ok();
        }

        public IActionResult Query(Models.PageParam<Models.Form.Condition> pageParam)
        {
            var result = this.FormManager.Query(pageParam);
            return this.Json(result);
        }

        [HttpGet]
        public IActionResult Design(string id)
        {
            var entity = this.FormManager.Find(id);
            if (entity == null)
            {
                entity = new Entities.Form() { ID = this.FormManager.GenerateMongoID };
            }
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult Design(string id, string html)
        {
            this.FormManager.SaveHtml(id, html);
            var entity = this.FormManager.Find(id);
            return this.View(entity);
        }


        [HttpGet]
        public IActionResult Fill(string id, string dataID)
        {
            var entity = this.FormManager.Find(id);
            var data = this.DynamicFormManager.Find(id, dataID);
            if (data == null)
                dataID = this.DynamicFormManager.GenerateMongoID;
            this.ViewData["id"] = id;
            this.ViewData["DataID"] = dataID;
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult Fill(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            this.DynamicFormManager.Save(id, dataID, dictionary);
            return this.Ok();
        }

        public IActionResult GetDynamicData(string id, string dataID)
        {
            var data = this.DynamicFormManager.Find(id, dataID);
            return this.Json(data?.ToDictionary());
        }

    }
}
