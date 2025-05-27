using System.Collections.Generic;
using PaymentApp.Models;

namespace PaymentApp.Interfaces
{
    // Repository abstraction providing methods to store and retrieve payments
    public interface IPaymentRepository
    {
        Payment Add(Payment payment);
        Payment GetById(int id);
        IEnumerable<Payment> GetAll();
    }
}
