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
            return View();
        }
    }
}