using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Models.DTOs;

namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponseDto> CreateAccountAsync(AccountCreateDto dto);
        Task<AccountResponseDto> GetAccountAsync(int accountId);
        Task<IEnumerable<AccountResponseDto>> GetAllAccountsAsync();
    }
}
