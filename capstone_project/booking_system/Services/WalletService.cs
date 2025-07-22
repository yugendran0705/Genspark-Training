namespace BookingSystem.Services;

using BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Repositories;
using System;
using System.Threading.Tasks;

public class WalletService : IWalletService
{
    private readonly int expiryDays = 180; // Balance expires after 180 days
    private readonly IRepository<string, Wallet> _walletRepository;

    public WalletService(IRepository<string, Wallet> walletRepository)
    {
        _walletRepository = walletRepository;
    }


    public async Task<Wallet> GetWalletByEmail(string email)
    {
        var wallet = await _walletRepository.Get(email);
        if (IsExpired(wallet.LastUpdated))
        {
            wallet.balance = 0;
            wallet.LastUpdated = DateTime.UtcNow;
            await _walletRepository.Update(wallet.CustomerEmail, wallet);
        }
        return wallet;
    }

    public async Task<Wallet> AddAmountToWallet(string email, int amount)
    {
        var wallet = await GetWalletByEmail(email); // Expiration check included
        wallet.balance += amount;
        wallet.LastUpdated = DateTime.UtcNow;
        return await _walletRepository.Update(wallet.CustomerEmail, wallet);
    }

    public async Task<Wallet> DeductAmountFromWallet(string email, int amount)
    {
        var wallet = await GetWalletByEmail(email); // Expiration check included
        if (wallet.balance < amount)
            throw new Exception("Insufficient wallet balance.");
        
        wallet.balance -= amount;
        wallet.LastUpdated = DateTime.UtcNow;
        return await _walletRepository.Update(wallet.CustomerEmail, wallet);
    }

    private bool IsExpired(DateTime lastUpdated)
    {
        return (DateTime.UtcNow - lastUpdated).TotalDays > expiryDays;
    }
}
