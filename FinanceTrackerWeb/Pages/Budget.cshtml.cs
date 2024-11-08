using FinanceTrackerWeb.Contracts;
using FinanceTrackerWeb.Data;
using FinanceTrackerWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceTrackerWeb.Pages
{
    [Authorize]
    public class BudgetModel : PageModel
    {
        private readonly ISpendingService _spendingService;
        private readonly UserManager<User> _userManager;
        private readonly FinanceContext _context;

        [BindProperty]
        public double Budget { get; set; }

        public BudgetModel(ISpendingService spendingService, UserManager<User> userManager, FinanceContext context)
        {
            _spendingService = spendingService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if(User.Identity.IsAuthenticated)
            {
                Budget = await _spendingService.GetBudgetAsync(User);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if(ModelState.IsValid)
            {
                currentUser.Budget = Budget;

                await _context.SaveChangesAsync();
            }


            return RedirectToPage();
        }
    }
}
