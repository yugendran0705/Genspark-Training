using BankApp.Interfaces;
using BankApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        [HttpPost]
        public async Task<IActionResult> AskFaq([FromBody] FaqRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required.");

            var answer = await _faqService.AskQuestionAsync(request.Question);
            return Ok(new { answer });
        }
    }
}
