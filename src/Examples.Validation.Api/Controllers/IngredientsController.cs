using System;
using System.ComponentModel.DataAnnotations;
using Examples.Validation.Api.Core.ExtensionMethods;
using Examples.Validation.Api.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using StructureMap;

using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Examples.Validation.Api.Controllers
{
    [Route("api/ingredients")]
    public class IngredientsController : ControllerBase
    {
        #region Member Variables
        private readonly IContainer _container;
        #endregion

        #region Constructor
        public IngredientsController(IContainer container)
        {
            // This is a pretty bad idea.  Injecting the IoC container into the
            // object pretty much defeats the purpose of DI and makes testing a
            // pain.
            _container = container;
        }
        #endregion

        [HttpGet("{id}", Name="GetIngredient")]
        public IActionResult Get(Guid id)
        {
            return Ok(new {Name = "Butter"});
        }

        [HttpPost]
        public IActionResult Post([FromBody,Required] AddIngredient command)
        {
            // This works and give me a reason to write an extension method 
            // (something I don't do very often); however, it's clunky at best
            // and forces me to inject the IoC container unless I inject every
            // command / query this controller might handle.
            if(command == null)
                return BadRequest();

            ValidationResult result = _container.TryGetValidatorInstance<AddIngredient>()?.Validate(command);

            if(result?.IsValid == false)
                return BadRequest(result);

            return CreatedAtRoute("GetIngredient", new {id = Guid.NewGuid()}, null);
        }
    }
}
