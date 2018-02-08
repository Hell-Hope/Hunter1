using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hunter.WebUI.ActionFilters
{
    public class ModelStateErrorFilterAttribute : ResultFilterAttribute
    {
        public static Models.InvalidResult<Dictionary<string, List<string>>> ToInvalidResult(ModelStateDictionary modelState)
        {
            var dictionary = new Dictionary<string, List<string>>();
            var result = Models.Result.CreateInvalidResult(dictionary);
            string firstMessage = null;
            foreach (var item in modelState)
            {
                var list = new List<string>();
                foreach (var error in item.Value.Errors)
                {
                    if (firstMessage == null)
                        result.Message = firstMessage = error.ErrorMessage;
                    list.Add(error.ErrorMessage);
                }
                dictionary[item.Key] = list;
            }
            result.Message = firstMessage;
            return result;
        }

        public static IActionResult Validate(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return null;
            var result = ToInvalidResult(modelState);
            return new BadRequestObjectResult(result);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            var result = Validate(context.ModelState);
            if (result != null)
                context.Result = result;
        }

        
    }
}
