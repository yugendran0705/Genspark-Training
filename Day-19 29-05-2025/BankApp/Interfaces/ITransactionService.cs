using System.Threading.Tasks;
using BankApp.Models.DTOs;

namespace BankApp.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> DepositAsync(DepositRequestDto dto);
        Task<bool> WithdrawAsync(WithdrawRequestDto dto);
        Task<bool> TransferAsync(TransferRequestDto dto);
    }
}
