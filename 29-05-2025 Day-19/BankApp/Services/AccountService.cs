using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Models.DTOs;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponseDto> CreateAccountAsync(AccountCreateDto dto)
        {
            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                HolderName = dto.HolderName,
                Balance = dto.InitialDeposit
            };

            account = await _accountRepository.AddAsync(account);

            return new AccountResponseDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                HolderName = account.HolderName,
                Balance = account.Balance
            };
        }

        public async Task<AccountResponseDto> GetAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return null;
            return new AccountResponseDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                HolderName = account.HolderName,
                Balance = account.Balance
            };
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(a => new AccountResponseDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                HolderName = a.HolderName,
                Balance = a.Balance
            });
        }
    }
}
