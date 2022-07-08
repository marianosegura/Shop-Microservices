using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastucture;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastucture.Mail;
using Ordering.Infrastucture.Persistence;
using Ordering.Infrastucture.Repositories;


namespace Ordering.Infrastucture
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // inject orders EF database
            services.AddDbContext<OrderContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"))
            );  

            // this is how classes with generics are injected
            // these are required for MediatR
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));  
            services.AddScoped<IOrderRepository, OrderRepository>();  // inject the "order table"

            // SendGrid email dependencies
            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
