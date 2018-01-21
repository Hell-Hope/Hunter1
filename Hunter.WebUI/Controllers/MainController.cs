using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}