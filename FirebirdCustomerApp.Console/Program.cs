using FirebirdCustomerApp.Application.Contracts;
using FirebirdCustomerApp.Application.DTOs.Customer;
using FirebirdCustomerApp.Application.Services;
using FirebirdCustomerApp.Infrastructure.Repositories;
using FirebirdSql.Data.FirebirdClient;

var csBuilder = new FbConnectionStringBuilder
{
    Database = @"localhost:C:\Users\jakub\Desktop\dev\firebird\TEST.FDB",
    UserID = "SYSDBA",
    Password = "masterkey",
    Charset = "NONE",
    ServerType = FbServerType.Default,
    Dialect = 3
};

string connectionString = csBuilder.ToString();

var repo = new FirebirdCustomerRepository(connectionString);
ICustomerService customerService = new CustomerService(repo);

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Customer Management ===");
    Console.WriteLine("1. Pokaż klientów");
    Console.WriteLine("2. Dodaj klienta");
    Console.WriteLine("3. Edytuj klienta");
    Console.WriteLine("4. Usuń klienta");
    Console.WriteLine("0. Wyjście");
    Console.Write("\nWybierz opcję: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await ShowCustomers(customerService);
                break;

            case "2":
                await AddCustomer(customerService);
                break;

            case "3":
                await EditCustomer(customerService);
                break;

            case "4":
                await DeleteCustomer(customerService);
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Nieznana opcja. Naciśnij klawisz, aby kontynuować...");
                Console.ReadKey();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ Wystąpił błąd: {ex.Message}");
        Console.WriteLine("Naciśnij klawisz, aby kontynuować...");
        Console.ReadKey();
    }
}

static async Task ShowCustomers(ICustomerService customerService)
{
    Console.Clear();
    Console.WriteLine("=== Lista klientów ===\n");

    var customers = await customerService.GetAllAsync();

    if (customers.Count == 0)
    {
        Console.WriteLine("Brak klientów w bazie.");
    }
    else
    {
        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Id}: {c.Name} | {c.City}");
        }
    }

    Console.WriteLine("\nNaciśnij klawisz, aby wrócić do menu...");
    Console.ReadKey();
}

static async Task AddCustomer(ICustomerService customerService)
{
    Console.Clear();
    Console.WriteLine("=== Dodaj klienta ===");

    Console.Write("Nazwa: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("Miasto: ");
    var city = Console.ReadLine() ?? string.Empty;

    var dto = new CreateCustomerDto
    {
        Name = name,
        City = city
    };

    await customerService.AddAsync(dto);

    Console.WriteLine("\n✅ Klient dodany.");
    Console.WriteLine("Naciśnij klawisz, aby wrócić do menu...");
    Console.ReadKey();
}

static async Task EditCustomer(ICustomerService customerService)
{
    Console.Clear();
    Console.WriteLine("=== Edytuj klienta ===");

    Console.Write("Podaj ID klienta: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("❌ Nieprawidłowe ID.");
        Console.ReadKey();
        return;
    }

    Console.Write("Nowa nazwa: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("Nowe miasto: ");
    var city = Console.ReadLine() ?? string.Empty;

    var dto = new UpdateCustomerDto
    {
        Id = id,
        Name = name,
        City = city
    };

    await customerService.UpdateAsync(dto);

    Console.WriteLine("\n✅ Klient zaktualizowany.");
    Console.WriteLine("Naciśnij klawisz, aby wrócić do menu...");
    Console.ReadKey();
}

static async Task DeleteCustomer(ICustomerService customerService)
{
    Console.Clear();
    Console.WriteLine("=== Usuń klienta ===");

    Console.Write("Podaj ID klienta: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("❌ Nieprawidłowe ID.");
        Console.ReadKey();
        return;
    }

    await customerService.DeleteAsync(id);

    Console.WriteLine("\n✅ Klient usunięty.");
    Console.WriteLine("Naciśnij klawisz, aby wrócić do menu...");
    Console.ReadKey();
}