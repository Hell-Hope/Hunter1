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
        public FormController(Manager manager)
        {
            this.Manager = manager;       
        }
        
        protected Manager Manager { get; set; }
       

        public IActionResult List()
        {
            return this.View();
        }

        public IActionResult Edit(string id)
        {
            Models.Form.Edit edit = this.Manager.FormManager.GetEdit(id) ?? new Models.Form.Edit();
            if (String.IsNullOrWhiteSpace(edit.ID))
                edit.ID = this.Manager.FormManager.GenerateMongoID;
            return this.View(edit);
        }

        public IActionResult Save([FromBody]Models.Form.Edit edit)
        {
            this.Manager.FormManager.Save(edit);
            return this.Ok();
        }

        public IActionResult Query([FromBody]Models.PageParam<Models.Form.Condition> pageParam)
        {
            var result = this.Manager.FormManager.Query(pageParam);
            return this.Json(result);
        }

        [HttpGet]
        public IActionResult Design(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            if (entity == null)
            {
                entity = new Entities.Form() { ID = this.Manager.FormManager.GenerateMongoID };
            }
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult Design(string id, string html)
        {
            this.Manager.FormManager.SaveHtml(id, html);
            var entity = this.Manager.FormManager.Find(id);
            return this.View(entity);
        }

        [HttpGet]
        public IActionResult Columns(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            return this.View(entity);
        }

        public IActionResult SaveColumns(string id, [FromBody]List<Dictionary<string, object>> list)
        {
            this.Manager.FormManager.SaveColumns(id, list);
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            this.Manager.FormManager.Remove(id);
            return this.Ok();
        }

    }
}
