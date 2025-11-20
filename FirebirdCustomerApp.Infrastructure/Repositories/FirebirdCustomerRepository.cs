using FirebirdCustomerApp.Domain.Entities;
using FirebirdCustomerApp.Domain.Repositories;
using FirebirdSql.Data.FirebirdClient;

namespace FirebirdCustomerApp.Infrastructure.Repositories;

public class FirebirdCustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public FirebirdCustomerRepository(string connectionString)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
    }
    public async Task<List<Customer>> GetAllAsync()
    {
        var customers = new List<Customer>();

        await using var conn = new FbConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = "SELECT Id, Name, City, CreatedAt FROM Customers ORDER BY Id";

        await using var cmd = new FbCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var customer = new Customer
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                City = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            };

            customers.Add(customer);
        }
        return customers;
    }

    public async Task AddAsync(Customer customer)
    {
        await using var conn = new FbConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = @"INSERT INTO Customers (Name, City) VALUES (@name, @city);";

        await using var cmd = new FbCommand(sql, conn);

        cmd.Parameters.AddWithValue("@name", customer.Name);
        cmd.Parameters.AddWithValue("@city", customer.City);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        await using var conn = new FbConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = @"UPDATE Customers 
                             SET Name = @name, City = @city 
                             WHERE Id = @id;";

        await using var cmd = new FbCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", customer.Id);
        cmd.Parameters.AddWithValue("@name", customer.Name);
        cmd.Parameters.AddWithValue("@city", customer.City);

        await cmd.ExecuteNonQueryAsync();
    }
    public async Task DeleteAsync(int id)
    {
        await using var conn = new FbConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = "DELETE FROM Customers WHERE Id = @id;";

        await using var cmd = new FbCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await cmd.ExecuteNonQueryAsync();
    }
}
