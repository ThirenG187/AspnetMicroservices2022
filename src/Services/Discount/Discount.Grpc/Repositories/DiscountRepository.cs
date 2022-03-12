using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = _ConnectionFactory();

        var affected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName,@Description,@Amount)",
            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount }
        );

        return !(affected == 0);
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = _ConnectionFactory();

        var affected = await connection.ExecuteAsync
            ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

        if (affected == 0) return false;

        return true;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var connection = _ConnectionFactory();

        string command = $@"
            SELECT Id, ProductName, Description, Amount
            FROM Coupon
            WHERE ProductName = @ProductName
        ";

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(command, new { ProductName = productName });

        return coupon ??
            new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = _ConnectionFactory();

        var affected = await connection.ExecuteAsync
            ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

        if (affected == 0) return false;

        return true;
    }

    private NpgsqlConnection _ConnectionFactory(string connectionString = "DatabaseSettings:ConnectionString")
    {
        return new NpgsqlConnection(_configuration.GetValue<string>(connectionString));
    }
}