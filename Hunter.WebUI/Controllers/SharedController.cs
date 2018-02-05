using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hunter.WebUI.Controllers
{
    [Authorize]
    public class SharedController : Controller
    {

        public SharedController(Managers.Manager manager)
        {
            this.Manager = manager;
            this.Manager.GetApplicationUser = () => this.ApplicationUser;
        }
        

        protected Managers.Manager Manager { get; set; }

        protected Models.ApplicationUser ApplicationUser
        {
            get
            {
                return this.User.GetApplicationUser();
            }
        }

        public IActionResult ActionResult(Models.Result result)
        {
            if (result.Success)
                return new OkObjectResult(result);
            return new BadRequestObjectResult(result);
        }

    }
}
