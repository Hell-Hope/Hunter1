using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hunter
{
    public static class ResultExtension
    {

        public static ActionResult ToActionResult(this Models.Result result)
        {
            if (result.Success)
                return new OkObjectResult(result);
            return new BadRequestObjectResult(result);
        }

    }
}
