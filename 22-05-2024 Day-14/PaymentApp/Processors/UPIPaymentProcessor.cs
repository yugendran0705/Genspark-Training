using System;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Processors
{
    // A concrete payment processor for UPI payments.
    public class UPIPaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Payment payment)
        {
            // Simulate processing UPI payment.
            Console.WriteLine($"Processing UPI payment for amount {payment.Amount:C}");
            return true;
        }
    }
}
