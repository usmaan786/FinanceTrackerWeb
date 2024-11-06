using Azure.Core;
using FinanceTrackerWeb.Data;
using FinanceTrackerWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLitePCL;
using System.Data.SqlTypes;

namespace FinanceTrackerWeb.Services
{
    [Route("api/plaid")]
    [ApiController]
    public class PlaidController : ControllerBase
    {
        private readonly PlaidService _plaidService;
        private readonly FinanceContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PlaidController> _logger;

        public PlaidController(PlaidService plaidService, FinanceContext context, UserManager<User> userManager, ILogger<PlaidController> logger)
        {
            _plaidService = plaidService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("create_link_token")]
        public async Task<IActionResult> CreateLinkToken()
        {
            var linkToken = await _plaidService.CreateLinkTokenAsync();
            return Ok(new { link_token = linkToken });
        }

        [HttpPost("exchange_token")]
        public async Task<IActionResult> ExchangeToken([FromBody] PublicTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.PublicToken))
            {
                return BadRequest("Public token is required");
            }
            var accessToken = await _plaidService.ExchangePublicTokenAsync(request.PublicToken);

            return Ok(new { AccessToken = accessToken });
            
        }

        [HttpPost("sync_transactions")]
        public async Task<IActionResult> SyncTransactions([FromBody] SyncTransactionsRequest request)
        {
            try
            {
                var transactions = await _plaidService.GetTransactionsAsync(request.AccessToken, request.StartDate, request.EndDate);

                var spendings = transactions.Select(transaction => new Spending
                {
                    Item = transaction.Name,
                    Spent = transaction.Amount,
                    TransactionDate = DateTime.Parse(transaction.Date),
                    UserId = _userManager.GetUserId(User)
                }).ToList();

                await _context.Spendings.AddRangeAsync(spendings);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            
        }
    }

    public class PublicTokenRequest
    {
        public string PublicToken { get; set; }
    }

    public class SyncTransactionsRequest
    {
        public string AccessToken { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
