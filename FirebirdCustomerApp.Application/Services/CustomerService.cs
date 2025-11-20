using FirebirdCustomerApp.Application.Contracts;
using FirebirdCustomerApp.Application.DTOs.Customer;
using FirebirdCustomerApp.Domain.Entities;
using FirebirdCustomerApp.Domain.Repositories;

namespace FirebirdCustomerApp.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public Task<List<Customer>> GetAllAsync() => _repo.GetAllAsync();

    public async Task AddAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer
        {
            Name = createCustomerDto.Name.Trim(),
            City = createCustomerDto.City.Trim(),
        };

        await _repo.AddAsync(customer);
    }

    public async Task UpdateAsync(UpdateCustomerDto updateCustomerDto)
    {
        if (updateCustomerDto.Id <= 0)
            throw new ArgumentException("Invalid customer ID");

        var customer = new Customer
        {
            Id = updateCustomerDto.Id,
            Name = updateCustomerDto.Name.Trim(),
            City = updateCustomerDto.City.Trim(),
        };

        await _repo.UpdateAsync(customer);
    }

    public Task DeleteAsync(int id)
    {
        if(id <= 0) throw new ArgumentException("invalid customer ID");

        return _repo.DeleteAsync(id);
    }
}
