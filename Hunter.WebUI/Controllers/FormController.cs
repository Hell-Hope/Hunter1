﻿using System;
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
            var design = this.Manager.FormManager.GetDesign(id);
            if (design == null)
            {

            }
            this.ModelState.Clear();
            return this.View(design);
        }

        [HttpPost]
        public IActionResult Design(Models.Form.Design design)
        {
            this.Manager.FormManager.Save(design);
            return this.View(design);
        }

        public IActionResult FlowChart(string id)
        {
            var model = this.Manager.FormManager.GetFlowChart(id);
            if (model == null)
            {
                return this.NotFound();
            }
            if (String.Equals("post", this.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                return this.Ok(model);
            }
            this.ModelState.Clear();
            this.ViewData["Fields"] = this.Manager.FormManager.GetFields(id) ?? new List<Models.Form.Field>();
            this.ViewData["Permits"] = this.Manager.PermitManager.GetAllForChoose();
            return this.View(model);
        }

        [HttpPost]
        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult SaveFlowChart([FromBody]Models.Form.FlowChart flowChart)
        {
            this.Manager.FormManager.Save(flowChart);
            return this.Ok();
        }

        [HttpGet]
        public IActionResult Columns(string id)
        {
            this.ModelState.Clear();
            this.ViewData["Fields"] = this.Manager.FormManager.GetFields(id) ?? new List<Models.Form.Field>();
            this.ViewData["Columns"] = this.Manager.FormManager.GetColumns(id) ?? new List<Models.Form.Column>();
            this.ViewData["ID"] = id;
            return this.View();
        }

        public IActionResult SaveColumns(string id, [FromBody]List<Models.Form.Column> list)
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
