using System;
using FluentValidation;

namespace Examples.Validation.Api.Domain.Commands
{
    public class AddReview
    {
        public Guid Id {get; set;}
        public Guid CreatedBy {get; set;}
        public int Rating {get; set;}
        public string Description {get; set;}
    }

    public class AddReviewValidator : AbstractValidator<AddReview>
    {
        public AddReviewValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
            RuleFor(cmd => cmd.CreatedBy).NotEmpty().NotEqual(cmd => cmd.Id);
            RuleFor(cmd => cmd.Rating).InclusiveBetween(1, 5);
        }
    }
}
