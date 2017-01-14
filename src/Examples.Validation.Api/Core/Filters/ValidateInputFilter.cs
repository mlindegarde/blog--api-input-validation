using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Examples.Validation.Api.Core.Filters
{
    public class ValidateInputFilter : IActionFilter
    {
        #region Member Variables
        private static readonly Dictionary<string,string> ClaimsToAdd = new Dictionary<string,string>
        {
            {"name", "Name"},
            {"role", "Role"}
        };

        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public ValidateInputFilter(ILogger logger)
        {
            _logger = logger.ForContext<ValidateInputFilter>();
        }
        #endregion

        #region IActionFilter Implementation
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid)
                return;

            // Normally I use Elasticsearch to handle error logging.  When that
            // sink is setup using the LogContext allows me to record some extra
            // information about the current request.
            using(LogContext.PushProperties(BuildIdentityEnrichers(context.HttpContext.User)))
            {
                _logger.Warning("Model validation failed for {@Input} with validation {@Errors}",
                    context.ActionArguments,
                    context.ModelState?
                        .SelectMany(kvp => kvp.Value.Errors)
                        .Select(e => e.ErrorMessage));
            }

            // I split validation errors into two categories: Input and Domain. 
            // - Input:  High level validation like ensuring required fields are
            //           not null and that certain data fields are correctly
            //           formatted (e.g. email address or phone number)
            // - Domain: Validation errors that depend on the state of the mode.
            //           For example, the current user doesn't have permission to
            //           post reviews.
            //
            // In both cases a 400 response is sent back to the client.  The
            // "Type" field is used to indicate where the error happened.
            context.Result = new BadRequestObjectResult(
                from kvp in context.ModelState
                from e in kvp.Value.Errors
                let k = kvp.Key
                select new ValidationError(ValidationError.Type.Input, null, k, e.ErrorMessage));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This filter doesn't do anything post action.
        }
        #endregion

        #region Utilty Methods
        protected ILogEventEnricher[] BuildIdentityEnrichers(ClaimsPrincipal user)
        {
            return ClaimsToAdd.Select(kvp => 
                new PropertyEnricher(
                    kvp.Value, 
                    user.Claims?.SingleOrDefault(c => c.Type == kvp.Key)?.Value)).ToArray<ILogEventEnricher>();
        }
        #endregion
    }
}
