using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace Examples.Validation.Api.Core.Configuration
{
    public static class StructureMapConfig
    {
        public static IServiceProvider AddStructureMap(this IServiceCollection services)
        {
            var container = new Container();

            container.Configure(_ =>
            {
                _.Scan(s =>
                {
                    // Let StructureMap magically map everything it can.
                    s.TheCallingAssembly();
                    s.WithDefaultConventions();
                    // Remove the line below if you aren't using StructureMap directly
                    // to resolve your validators.
                    s.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                    s.AssembliesFromApplicationBaseDirectory(a => a.FullName.Contains("Examples"));
                });
            });

            // Grab everything already configured in the built in IoC container.
            container.Populate(services);
            return container.GetInstance<IServiceProvider>();
        }
    }
}
