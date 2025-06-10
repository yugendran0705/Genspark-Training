namespace BankApp.Models.DTOs
{
    public class TransferRequestDto
    {
        public int SourceAccountId { get; set; }
        public int TargetAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
