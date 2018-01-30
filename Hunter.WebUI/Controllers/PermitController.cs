using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class PermitController : Controller
    {
        public PermitController(Managers.Manager manager)
        {
            this.Manager = manager;
        }

        public Managers.Manager Manager { get; set; }



    }
}