using System;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Processors
{
    public class CreditCardPaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Payment payment)
        {
            Console.WriteLine($"Processing credit card payment for amount {payment.Amount}");
            return true; 
        }
    }
}
