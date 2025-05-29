using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankContext context) : base(context) { }
    }
}
