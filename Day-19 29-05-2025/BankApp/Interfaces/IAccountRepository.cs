using BankApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAsync(int id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> AddAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task<bool> DeleteAsync(int id);
    }
}
