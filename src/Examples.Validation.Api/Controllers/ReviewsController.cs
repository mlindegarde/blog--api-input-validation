using System;
using System.ComponentModel.DataAnnotations;
using Examples.Validation.Api.Core.Attributes;
using Examples.Validation.Api.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Validation.Api.Controllers
{
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        [HttpGet("{id}", Name="GetReview")]
        public IActionResult Get(Guid id)
        {
            return Ok(new {Rating = 3, Description = "Meh"});
        }

        // The attribute is a step forward.  I don't have to type the validation
        // code in every action handler; however, I can't inject my logger into
        // the attribute and I don't like using attributes for functionality.
        // I prefer to limit attributes to metadata (even when using AOP).
        [HttpPost]
        [ValidateInput]
        public IActionResult Post([FromBody,Required] AddReview command)
        {
            return CreatedAtRoute("GetReview", new {id = Guid.NewGuid()}, null);
        }
    }
}
