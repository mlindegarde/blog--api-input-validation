using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Validation.Api.Core.Attributes
{
    public class ValidateInputAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid)
                return;

            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
