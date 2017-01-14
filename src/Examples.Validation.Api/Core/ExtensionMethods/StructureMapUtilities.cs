using FluentValidation;
using StructureMap;

namespace Examples.Validation.Api.Core.ExtensionMethods
{
    public static class StructureMapUtilities
    {
        public static IValidator TryGetValidatorInstance<T>(this IContainer container)
        {
            return container.TryGetInstance<AbstractValidator<T>>();
        }
    }
}
