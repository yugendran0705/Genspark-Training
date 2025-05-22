using System.Collections.Generic;
using PaymentApp.Models;

namespace PaymentApp.Interfaces
{
    // Service interface for processing and retrieving payments
    public interface IPaymentService
    {
        int ProcessPayment(Payment payment, );
        IEnumerable<Payment> GetPaymentHistory();
    }
}
