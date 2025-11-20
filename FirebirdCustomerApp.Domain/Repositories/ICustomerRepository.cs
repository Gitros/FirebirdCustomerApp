using FirebirdCustomerApp.Domain.Entities;

namespace FirebirdCustomerApp.Domain.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}
