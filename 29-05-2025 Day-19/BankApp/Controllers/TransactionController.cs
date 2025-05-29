using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit([FromBody] DepositRequestDto dto)
        {
            var result = await _transactionService.DepositAsync(dto);
            if (!result)
                return BadRequest("Deposit failed. Account may not exist.");
            return Ok("Deposit successful.");
        }
        
        [HttpPost("withdraw")]
        public async Task<ActionResult> Withdraw([FromBody] WithdrawRequestDto dto)
        {
            var result = await _transactionService.WithdrawAsync(dto);
            if (!result)
                return BadRequest("Withdrawal failed. Check account details or balance.");
            return Ok("Withdrawal successful.");
        }
        
        [HttpPost("transfer")]
        public async Task<ActionResult> Transfer([FromBody] TransferRequestDto dto)
        {
            var result = await _transactionService.TransferAsync(dto);
            if (!result)
                return BadRequest("Transfer failed. Verify account details and balance.");
            return Ok("Transfer successful.");
        }
    }
}
