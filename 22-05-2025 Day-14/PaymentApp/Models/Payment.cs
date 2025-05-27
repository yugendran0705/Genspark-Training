using System;

namespace PaymentApp.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }

        public Payment(decimal amount, DateTime paymentDate, string paymentMethod)
        {
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            IsProcessed = false;
        }

        public override string ToString()
        {
            return $"Payment ID: {Id}, Amount: {Amount}, Date: {PaymentDate}, " +
                   $"Method: {PaymentMethod}, Processed: {IsProcessed}";
        }
    }
}
