using PaymentApp.Models;

namespace PaymentApp.Interfaces
{
    // Abstraction for payment processing (DIP, LSP)
    public interface IPaymentProcessor
    {
        bool ProcessPayment(Payment payment);
    }
}
