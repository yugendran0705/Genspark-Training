using System;
using PaymentApp.Models;
using PaymentApp.Repositories;
using PaymentApp.Services;
using PaymentApp.Interfaces;
using PaymentApp.Processors;

namespace PaymentApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPaymentProcessor cardPaymentProcessor = new CreditCardPaymentProcessor();

            IPaymentProcessor UPIPaymentProcessor = new UPIPaymentProcessor();
            IPaymentRepository paymentRepository = new PaymentRepository();
            IPaymentService paymentService;
            Console.WriteLine("Welcome to PaymentApp!");

            // Get payment details from the user.
            Console.Write("Enter payment amount: ");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Please enter a valid amount.");
            }

            Console.Write("Enter payment date and time (yyyy-MM-dd HH:mm): ");
            DateTime paymentDate;
            while (!DateTime.TryParse(Console.ReadLine(), out paymentDate))
            {
                Console.WriteLine("Please enter a valid date and time.");
            }

            Console.Write("Enter payment method (CreditCard/UPI): ");
            string method = Console.ReadLine() ?? "CreditCard";
            if (method != "CreditCard" && method != "UPI")
            {
                Console.WriteLine("Invalid payment method. Defaulting to CreditCard.");
                method = "CreditCard";
            }
            // Create and process the payment.
            Payment payment = new Payment(amount, paymentDate, method);
            paymentService = new PaymentService(paymentRepository, method == "CreditCard" ? cardPaymentProcessor : UPIPaymentProcessor);
            int paymentId = paymentService.ProcessPayment(payment);
            if (paymentId != -1)
            {
                Console.WriteLine($"Payment successfully processed with ID: {paymentId}");
            }
            else
            {
                Console.WriteLine("Payment failed to process.");
            }

            // Display payment history.
            Console.WriteLine("\nPayment History:");
            foreach (var p in paymentService.GetPaymentHistory())
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
