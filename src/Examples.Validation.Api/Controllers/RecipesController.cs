using System;
using System.ComponentModel.DataAnnotations;
using Examples.Validation.Api.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Validation.Api.Controllers
{
    [Route("api/recipes")]
    public class RecipesController : ControllerBase
    {
        [HttpGet("{id}", Name="GetRecipe")]
        public IActionResult Get(Guid id)
        {
            return Ok(new {Name = "Carrot Cake"});
        }

        // This is how it should be.  By using the filter I no longer have to worry
        // about validating the input.  If input validation fails the filter will take
        // care of logging the issue, setting the 400 status code, and returning the
        // validation errors to the caller.
        [HttpPost]
        public IActionResult Post([FromBody,Required] AddRecipe command)
        {
            return CreatedAtRoute("GetRecipe", new {id = Guid.NewGuid()}, null);
        }
    }
}
