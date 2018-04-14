using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class MainController : SharedController
    {
        public MainController(Managers.Manager manager) : base(manager)
        {
        }

        public IActionResult Index()
        {
            this.ViewData[nameof(ApplicationUser)] = this.ApplicationUser;
            this.ViewData["FormMenuItems"] = this.Manager.FormManager.GetMenuItems();
            return this.View();
        }

        

    }
}