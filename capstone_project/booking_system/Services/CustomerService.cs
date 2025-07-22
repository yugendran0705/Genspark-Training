namespace BookingSystem.Services;

using BookingSystem.Models;
using BookingSystem.Repositories;
using BookingSystem.Interfaces;
using BookingSystem.Models.DTOs;

public class CustomerService : ICustomerService
{
    private readonly IRepository<string, Customer> _customerRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IRepository<string, Wallet> _walletRepository;
    private readonly IEncryptionService _encryptionService;

    public CustomerService(IRepository<string, Customer> customerRepository, IRepository<string, User> userRepository, IEncryptionService encryptionService, IRepository<string, Wallet> walletRepository)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _walletRepository = walletRepository;
    }

    public async Task<Customer> RegisterCustomer(CustomerDto customerDto)
    {
        var existingUser = await _userRepository.Get(customerDto.Email);
        if (existingUser != null)
        {
            throw new Exception("A user with this email already exists.");
        }

        var encrypteddata = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = customerDto.Password
        });

        var user = new User
        {
            Name = customerDto.Name,
            Email = customerDto.Email,
            Password = encrypteddata.EncryptedData ?? "",
            Role = "Customer"
        };

        user = await _userRepository.Add(user);

        // Create customer before wallet
        var customer = new Customer
        {
            Name = customerDto.Name,
            Email = customerDto.Email,
            PhoneNumber = customerDto.PhoneNumber,
            Address = customerDto.Address
        };

        var newCustomer = await _customerRepository.Add(customer);

        // Now the customer exists, you can safely create wallet
        Wallet wallet = await _walletRepository.Add(new Wallet
        {
            CustomerEmail = customerDto.Email,
            balance = customerDto.WalletBalance,
            LastUpdated = DateTime.UtcNow
        });

        return newCustomer;
    }


    public async Task<Customer> UpdateCustomer(CustomerDto customerDto)
{
    
    var existingCustomer = await _customerRepository.Get(customerDto.Email);
    if (existingCustomer == null)
    {
        throw new Exception("Customer not found.");
    }

    
    existingCustomer.Name = customerDto.Name;
    existingCustomer.PhoneNumber = customerDto.PhoneNumber;
    existingCustomer.Address = customerDto.Address;

  
    await _customerRepository.Update(customerDto.Email, existingCustomer);

    return existingCustomer;
}

    public async Task<Customer> GetCustomerByName(string name)
    {
        return await _customerRepository.Get(name);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        return await _customerRepository.GetAll();
    }
}
