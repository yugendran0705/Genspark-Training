using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(BankContext context) : base(context) { }
    }
}
