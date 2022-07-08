using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System.Reflection;


namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {  // extension method to add all application project services at once
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());  // for object mapping (mainly from-to dtos)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());  // for CQRS request validators
            services.AddMediatR(Assembly.GetExecutingAssembly());  // for IRequestHandler child classes to work
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));  // inject pipeline behaviours to be available for injection requests
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>)); 
            
            return services;
        }
    }
}
