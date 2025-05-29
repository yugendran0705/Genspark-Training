using System.Collections.Generic;
using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromBody] AccountCreateDto accountDto)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.Id }, createdAccount);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseDto>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountAsync(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
    }
}
