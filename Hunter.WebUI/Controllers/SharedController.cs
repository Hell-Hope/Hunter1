using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.Manager.ApplicationUser = context.HttpContext.User.GetApplicationUser();
            this.Manager.ApplicationUser.Permits = this.Manager.UserManager.GetPermits(this.Manager.ApplicationUser.ID);
            base.OnActionExecuting(context);
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
