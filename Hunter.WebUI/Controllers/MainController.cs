using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class MainController : Controller
    {
        public MainController(Managers.Manager manager)
        {
            this.Manager = manager;
        }

        public Managers.Manager Manager { get; set; }

        public IActionResult Index()
        {
            this.ViewData["FormMenuItems"] = this.Manager.FormManager.GetMenuItems();
            return this.View();
        }
    }
}