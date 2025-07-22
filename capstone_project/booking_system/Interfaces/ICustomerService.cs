namespace BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Models.DTOs ;

public interface ICustomerService
{
    Task<Customer> RegisterCustomer(CustomerDto customerDto);
    Task<Customer> GetCustomerByName(string name);
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer> UpdateCustomer(CustomerDto customerDto);
}
