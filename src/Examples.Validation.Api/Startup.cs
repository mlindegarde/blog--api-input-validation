using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Examples.Validation.Api.Core.Configuration;
using Examples.Validation.Api.Core.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace Examples.Validation.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.local.json", optional: true);

            builder.AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Examples.Validation.Api")
                .Enrich.WithProperty("Version", PlatformServices.Default.Application.ApplicationVersion)
                .WriteTo.LiterateConsole()
                .CreateLogger();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                // Remove the filter if you prefer to use the attribute.
                .AddMvcOptions(options => options.Filters.Add(new ValidateInputFilter(Log.Logger)))
                // Remove the line below if you're going to resolve your validators
                // via StructureMap (probably a bad idea).
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            return services.AddStructureMap();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
