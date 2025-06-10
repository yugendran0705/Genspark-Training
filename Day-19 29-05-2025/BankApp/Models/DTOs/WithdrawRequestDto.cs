namespace BankApp.Models.DTOs
{
    public class WithdrawRequestDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
