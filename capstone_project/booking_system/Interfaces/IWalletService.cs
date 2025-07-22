namespace BookingSystem.Interfaces;

using BookingSystem.Models;
using System.Threading.Tasks;

public interface IWalletService
{
    Task<Wallet> GetWalletByEmail(string email);
    Task<Wallet> AddAmountToWallet(string email, int amount);
    Task<Wallet> DeductAmountFromWallet(string email, int amount);
}
