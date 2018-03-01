using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class DynamicFormController : SharedController
    {
        public DynamicFormController(IHostingEnvironment hostingEnvironment, Managers.Manager manager) : base(manager)
        {
            this.HostingEnvironment = hostingEnvironment;
        }

        protected IHostingEnvironment HostingEnvironment { get; set; }

        public IActionResult List(string id)
        {
            var columns = this.Manager.FormManager.GetColumns(id) ?? new List<Models.Form.Column>();
            this.ViewData["ID"] = id;
            this.ViewData["Columns"] = columns;
            return View();
        }

        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Query(string id, [FromBody]Models.PageParam<Models.DynamicForm.Condition> pageParam)
        {
            var result = this.Manager.DynamicFormManager.Query(id, pageParam);
            return this.ActionResult(result);
        }

        [HttpGet]
        public IActionResult Edit(string id, string dataID)
        {
            var entity = this.Manager.DynamicFormManager.Find(id, dataID);
            if (entity == null)
            {
                entity = new Entities.DynamicForm() { ID = this.Manager.GenerateMongoID };
                var form = this.Manager.FormManager.Find(id);
                this.Manager.DynamicFormManager.Copy(form, entity);
                entity.CurrentNode = entity.Nodes.GetStartNode();
                if (entity.CurrentNode == null)
                    return this.DisplayResult(Models.Result.CreateFail("缺少开始节点"));
            }
            var result = this.Manager.DynamicFormManager.HasPermit(entity);
            if (result.Success == false)
                return this.DisplayResult(result);
            this.ViewData["id"] = id;
            this.ViewData["DataID"] = entity.ID;
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult SaveData(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            var dic = new Dictionary<string, object>(dictionary.Count);
            foreach (var item in dictionary)
            {
                if (item.Value is Newtonsoft.Json.Linq.JArray jarray)
                {
                    dic[item.Key] = jarray.ToObject<object[]>();
                }
                else
                {
                    dic[item.Key] = item.Value;
                }
            }
            var result = this.Manager.DynamicFormManager.SaveData(id, dataID, dic);
            return this.ActionResult(result);
        }

        public IActionResult Next(string id, string dataID, string lineID)
        {
            var result = this.Manager.DynamicFormManager.Next(id, dataID, lineID);
            return this.ActionResult(result);
        }

        [ActionFilters.ModelStateErrorFilterAttribute]
        public IActionResult Finish(string id, string dataID)
        {
            var result = this.Manager.DynamicFormManager.Finish(id, dataID);
            return this.ActionResult(result);
        }

        public IActionResult Progress(string id, string dataID)
        {
            var progress = this.Manager.DynamicFormManager.GetProgress(id, dataID);
            return this.View(progress);
        }

        public IActionResult Find(string id, string dataID)
        {
            var data = this.Manager.DynamicFormManager.Find(id, dataID);
            return this.Ok(data);
        }

        public IActionResult Remove(string id, string dataID)
        {
            this.Manager.DynamicFormManager.Remove(id, dataID);
            return this.Ok();
        }

        public IActionResult Download(string id, string dataID, string type)
        {
            var entity = this.Manager.DynamicFormManager.Find(id, dataID);
            var html = this.Manager.DynamicFormManager.GetCompleteHtml(entity.Html, entity.Data, this.HostingEnvironment.WebRootPath);
            if (String.Equals("pdf", type, StringComparison.OrdinalIgnoreCase))
            {
                var stream = new System.IO.MemoryStream();
                var fontProvider = new iText.Html2pdf.Resolver.Font.DefaultFontProvider(true, true, true);
                var converterProperties = new iText.Html2pdf.ConverterProperties();
                converterProperties.SetFontProvider(fontProvider);
                //converterProperties.SetCreateAcroForm(true);
                
                iText.Html2pdf.HtmlConverter.ConvertToPdf(html, stream, converterProperties);
                var bytes = stream.ToArray();
                return this.File(bytes, "application/pdf");
            }
            return this.Content(html, "text/html");
        }



    }
}