using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;


namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {  // extension method to create the DiscountDb database and the Coupon table (and seeding some rows)
        public static IHost MigratePostgreSQLDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;  // nullable object value (0 if null I guess)

            using (var scope = host.Services.CreateScope())
            {  // create temporal scope to access injected services
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating PostgreSQL database.");
                    using var connection = new NpgsqlConnection(
                        configuration.GetValue<string>("DatabaseSettings:ConnectionString")    
                    );
                    connection.Open();

                    using var command = new NpgsqlCommand{ Connection = connection };
                    
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();  // delete table if required

                    command.CommandText = @"CREATE TABLE Coupon ( 
                        ID          SERIAL PRIMARY KEY NOT NULL, 
                        ProductName VARCHAR(24) NOT NULL, 
                        Description TEXT, 
                        Amount      INT 
                    );";
                    command.ExecuteNonQuery();  // create table

                    command.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES 
                        ('IPhone X', 'IPhone Discount', 150),
                        ('Samsung 10', 'Samsung Discount', 100)
                    ;";
                    command.ExecuteNonQuery();  // seed data

                    logger.LogInformation("Migrated PostgreSQL database.");
                }
                catch (NpgsqlException error)
                {
                    logger.LogError(error, "An error ocurred while migrating the PostgreSQL database");
                    if (retryForAvailability < 50)  // retry up to n times to migrate db (db container may not be ready immediately at this api startup)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);  // sleep 2s
                        MigratePostgreSQLDatabase<TContext>(host, retryForAvailability);  // retry the above code
                    }
                }
                return host;
            }
        }

    }
}
