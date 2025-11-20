namespace FirebirdCustomerApp.Application.DTOs.Customer;

public class UpdateCustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
}
