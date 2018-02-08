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
    public class FormController : SharedController
    {
        public FormController(Manager manager) : base(manager)
        { 
        }
        
       

        public IActionResult List()
        {
            return this.View();
        }

        public IActionResult Edit(string id)
        {
            var edit = this.Manager.FormManager.GetEdit(id) ?? new Models.Form.Edit();
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

        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Save([FromBody]Models.Form.Edit edit)
        {
            this.Manager.FormManager.Save(edit);
            return this.Ok();
        }

        [ActionFilters.ModelStateErrorFilterAttribute]
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

        public IActionResult FlowChart(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            if (entity == null)
            {
                return this.NotFound();
            }
            if (String.Equals("post", this.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                var model = this.Manager.FormManager.Convert(entity);
                return this.Ok(model);
            }
            this.ViewData["ID"] = id;
            return this.View(entity);
        }

        [HttpPost]
        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult SaveFlowChart(string id, [FromBody]Models.Form.FlowChart flowChart)
        {
            this.Manager.FormManager.SaveFlowChart(id, flowChart);
            return this.Ok();
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
