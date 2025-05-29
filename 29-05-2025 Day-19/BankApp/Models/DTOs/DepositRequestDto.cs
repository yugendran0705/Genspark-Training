namespace BankApp.Models.DTOs
{
    public class DepositRequestDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
