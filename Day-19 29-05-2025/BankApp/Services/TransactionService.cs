using System;
using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Models.DTOs;

namespace BankApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> DepositAsync(DepositRequestDto dto)
        {
            var account = await _accountRepository.GetAsync(dto.AccountId);
            if (account == null)
                return false;

            account.Balance += dto.Amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Deposit,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }

        public async Task<bool> WithdrawAsync(WithdrawRequestDto dto)
        {
            var account = await _accountRepository.GetAsync(dto.AccountId);
            if (account == null || account.Balance < dto.Amount)
                return false;

            account.Balance -= dto.Amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Withdrawal,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }

        public async Task<bool> TransferAsync(TransferRequestDto dto)
        {
            var sourceAccount = await _accountRepository.GetAsync(dto.SourceAccountId);
            var targetAccount = await _accountRepository.GetAsync(dto.TargetAccountId);
            if (sourceAccount == null || targetAccount == null || sourceAccount.Balance < dto.Amount)
                return false;

            sourceAccount.Balance -= dto.Amount;
            targetAccount.Balance += dto.Amount;
            await _accountRepository.UpdateAsync(sourceAccount);
            await _accountRepository.UpdateAsync(targetAccount);

            var transaction = new Transaction
            {
                AccountId = sourceAccount.Id,
                Amount = dto.Amount,
                Type = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow,
                TargetAccountId = targetAccount.Id
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }
    }
}