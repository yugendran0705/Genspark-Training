using Microsoft.AspNetCore.Mvc;
using BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Wallet>> GetWalletByUserId(string email)
        {
            try
            {
                var wallet = await _walletService.GetWalletByEmail(email);
                if (wallet == null)
                    return NotFound("Wallet not found");

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallet for user ID: {email}", email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("add")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Wallet>> AddFunds([FromBody] WalletTransactionDto dto)
        {
            try
            {
                var updatedWallet = await _walletService.AddAmountToWallet(dto.email, dto.Amount);
                return Ok(updatedWallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding funds to wallet for user ID: {email}", dto.email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("deduct")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Wallet>> DeductFunds([FromBody] WalletTransactionDto dto)
        {
            try
            {
                var updatedWallet = await _walletService.DeductAmountFromWallet(dto.email, dto.Amount);
                if (updatedWallet == null)
                    return BadRequest("Insufficient balance");

                return Ok(updatedWallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting funds from wallet for user ID: {email}", dto.email);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
