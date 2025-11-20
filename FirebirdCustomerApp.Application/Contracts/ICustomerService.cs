using FirebirdCustomerApp.Application.DTOs.Customer;
using FirebirdCustomerApp.Domain.Entities;

namespace FirebirdCustomerApp.Application.Contracts;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();
    Task AddAsync(CreateCustomerDto createCustomerDto);
    Task UpdateAsync(UpdateCustomerDto updateCustomerDto);
    Task DeleteAsync(int id);
}
