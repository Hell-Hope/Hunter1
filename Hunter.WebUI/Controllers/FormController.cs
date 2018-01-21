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

        protected MongoClient MongoClient { get; private set; }

        protected FormManager FormManager { get; set; }

        protected DynamicFormManager DynamicFormManager { get; set; }

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
            var entity = this.FormManager.SaveHtml(id, html);
            return this.View(entity);
        }


        public IActionResult GetDynamicData(string id, string dataID)
        {
            var data = this.DynamicFormManager.Find(id, dataID);
            return this.Json(data?.ToDictionary());
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

    }
}
