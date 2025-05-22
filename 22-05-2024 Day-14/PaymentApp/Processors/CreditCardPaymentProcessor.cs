using System;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Processors
{
    // A concrete payment processor for Credit Card payments.
    public class CreditCardPaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Payment payment)
        {
            // Simulate processing a credit card payment.
            Console.WriteLine($"Processing credit card payment for amount {payment.Amount:C}");
            return true; // Assume payment is successful.
        }
    }
}
