
namespace BankApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // A single account can have many transactions.
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
