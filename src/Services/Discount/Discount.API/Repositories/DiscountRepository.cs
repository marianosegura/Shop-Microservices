using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;


namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;


        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@name, @description, @amount)",
                new { name=coupon.ProductName, description=coupon.Description, amount=coupon.Amount}
            );
            bool success = affected != 0;
            return success;
        }


        public async Task<Coupon> ReadDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(  // using for variable scoping (aka dropping the connection at method end)
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString")
            );
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @name",
                new { name=productName }  // pass query params as object
            );
            if (coupon == null)
            {
                return new Coupon
                {  // object initializer instead of regular constructor
                    ProductName = "No Discount",
                    Description = "Not Discount Desc",
                    Amount = 0
                };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@name, Description=@description, Amount=@amount WHERE ID=@id",
                new { name=coupon.ProductName, description=coupon.Description, amount=coupon.Amount, id=coupon.Id }
            );
            bool success = affected != 0;
            return success;
        }


        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName=@name",
                new { name=productName }
            );
            bool success = affected != 0;
            return success;
        }
    }
}
