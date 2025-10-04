using LavaChatBackend.Models.Accounts;
using LavaChatBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LavaChatBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _accountService.Authenticate(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("facebook-callback")]
        public async Task<IActionResult> FacebookCallback([FromQuery] string code, [FromQuery] string state)
        {
            // You can process the OAuth code here
            // (e.g., exchange code for access token, validate user, etc.)
            return Ok(new { message = "Facebook callback received", code, state });
        }


    }
}
