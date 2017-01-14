using System;
using FluentValidation;

namespace Examples.Validation.Api.Domain.Commands
{
    public class AddIngredient
    {
        public Guid Id {get; set;}
        public Guid CategoryId {get; set;}
        public Guid CreatedBy {get; set;}
        public DateTime CreatedAt {get ;set;}
        public string Name {get; set;}
    }

    public class AddIngredientValidator : AbstractValidator<AddIngredient>
    {
        private static readonly DateTime MinDate = new DateTime(2000, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(2100, 1, 1);

        public AddIngredientValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
            RuleFor(cmd => cmd.CategoryId).NotEmpty();
            RuleFor(cmd => cmd.CreatedBy).NotEmpty().NotEqual(cmd => cmd.Id);
            RuleFor(cmd => cmd.CreatedAt).Must(BeValidDate).WithMessage("'Created At' must be a valid date");
            RuleFor(cmd => cmd.Name).Length(5, 100);
        }

        private bool BeValidDate(DateTime input)
        {
            return input.Date > MinDate && input.Date < MaxDate;
        }
    }
}
