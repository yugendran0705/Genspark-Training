using System.Collections.Generic;
using System.Linq;
using PaymentApp.Interfaces;
using PaymentApp.Models;

namespace PaymentApp.Repositories
{
    // A simple in-memory repository for Payment objects.
    public class PaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments = new List<Payment>();
        private int _counter = 100; // Starting Payment Id

        public Payment Add(Payment payment)
        {
            payment.Id = ++_counter;
            _payments.Add(payment);
            return payment;
        }

        public IEnumerable<Payment> GetAll()
        {
            return _payments;
        }

        public Payment GetById(int id)
        {
            return _payments.FirstOrDefault(p => p.Id == id);
        }
    }
}
