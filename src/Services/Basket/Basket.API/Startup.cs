using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;


namespace Basket.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // our services
            // Redis
            string redisUrl = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            services.AddStackExchangeRedisCache(options =>  // connect redis db
            {
                options.Configuration = redisUrl;
            });

            // General 
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));
            
            // Grpc
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => 
                options.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"])
            );  // inject grpc client with it's url
            services.AddScoped<DiscountGrpcService>();

            //MassTransit - RabbitMQ
            services.AddMassTransit(massTransitConfiguration =>
            {  // setup rabbitmq connection through MassTransit
                massTransitConfiguration.UsingRabbitMq((context, configurator) =>
                {  // rabbitmq uses amqp protocol instead of http or other
                    configurator.Host(Configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
