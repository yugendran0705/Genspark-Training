using System;
using System.Collections.Generic;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Services
{
    // The PaymentService coordinates payment processing and storage.
    // It follows DIP by depending on abstractions (IPaymentRepository and IPaymentProcessor).
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentProcessor _paymentProcessor;

        public PaymentService(IPaymentRepository paymentRepository, IPaymentProcessor paymentProcessor)
        {
            _paymentRepository = paymentRepository;
            _paymentProcessor = paymentProcessor;
        }

        // Process a payment: store the payment and invoke the processor.
        public int ProcessPayment(Payment payment)
        {
            try
            {
                var addedPayment = _paymentRepository.Add(payment);
                if (addedPayment != null)
                {
                    bool success = _paymentProcessor.ProcessPayment(addedPayment);
                    if (success)
                    {
                        addedPayment.IsProcessed = true;
                        Console.WriteLine("Payment processed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Payment processing failed.");
                    }
                    return addedPayment.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing payment: " + ex.Message);
            }
            return -1;
        }

        public IEnumerable<Payment> GetPaymentHistory()
        {
            return _paymentRepository.GetAll();
        }
    }
}
