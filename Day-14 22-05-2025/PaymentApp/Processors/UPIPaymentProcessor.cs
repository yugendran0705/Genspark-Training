using System;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Processors
{
    public class UPIPaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Payment payment)
        {
            Console.WriteLine($"Processing UPI payment for amount {payment.Amount}");
            return true;
        }
    }
}
