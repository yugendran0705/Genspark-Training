using System;

namespace BankApp.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; } 
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? TargetAccountId { get; set; }
    }
}
