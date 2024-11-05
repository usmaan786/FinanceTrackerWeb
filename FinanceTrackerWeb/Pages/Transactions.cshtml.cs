using FinanceTrackerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTrackerWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinanceTrackerWeb.Pages
{
    [Authorize]
    public class TransactionsModel : PageModel
    {
        private readonly FinanceContext _context;
        private readonly UserManager<User> _userManager;
        private readonly HttpClient _httpClient;

        public TransactionsModel(FinanceContext context, UserManager<User> userManager, HttpClient httpClient)
        {
            _context = context;
            _userManager = userManager;
            _httpClient = httpClient;
        }

        public string LinkToken { get; set; }
        public List<Spending> Spendings { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return Challenge(); //redirect to login
            }
            Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");

            var response = await _httpClient.GetAsync("https://localhost:7190/api/plaid/create_link_token");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                LinkToken = result?["link_token"];
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            var spendings = _context.Spendings
                .Where(s => s.UserId == user.Id);

            Spendings = await spendings.ToListAsync();

            return Page();
        }
    }
}
