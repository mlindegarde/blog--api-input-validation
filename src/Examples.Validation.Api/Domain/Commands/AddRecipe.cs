using System;
using System.Collections.Generic;
using FluentValidation;

namespace Examples.Validation.Api.Domain.Commands
{
    public class AddRecipe
    {
        public Guid Id {get; set;}
        public Guid CreatedBy {get; set;}
        public DateTime CreatedAt {get ;set;}
        public string Title {get; set;}
        public string Instructions {get; set;}
        public List<string> Ingredients {get; set;}
    }

    public class AddRecipeValidator : AbstractValidator<AddRecipe>
    {
        private static readonly DateTime MinDate = new DateTime(2000, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(2100, 1, 1);

        public AddRecipeValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
            RuleFor(cmd => cmd.CreatedBy).NotEmpty().NotEqual(cmd => cmd.Id);
            RuleFor(cmd => cmd.CreatedAt).Must(BeValidDate).WithMessage("'Created At' must be a valid date");
            RuleFor(cmd => cmd.Title).Length(5, 100);
            RuleFor(cmd => cmd.Ingredients).NotEmpty();
        }

        private bool BeValidDate(DateTime input)
        {
            return input.Date > MinDate && input.Date < MaxDate;
        }
    }
}
