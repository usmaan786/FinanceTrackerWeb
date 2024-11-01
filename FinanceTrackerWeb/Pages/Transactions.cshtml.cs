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

        public TransactionsModel(FinanceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<Spending> Spendings { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return Challenge(); //redirect to login
            }

            var spendings = _context.Spendings
                .Where(s => s.UserId == user.Id);

            Spendings = await spendings.ToListAsync();

            return Page();
        }
    }
}
