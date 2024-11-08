using FinanceTrackerWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTrackerWeb.Models;
using FinanceTrackerWeb.Contracts;

namespace FinanceTrackerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISpendingService _spendingService;
        private readonly UserManager<User> _userManager;

        public Dictionary<string, double> MonthlySpendings { get; set; }
        public double TotalSpendings { get; set; }
        public double Budget { get; set; }
        public double BudgetPercentage { get; set; }
        public double OverBudget { get; set; }


        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager,ISpendingService spendingService)
        {
            _logger = logger;
            _userManager = userManager;
            _spendingService = spendingService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                MonthlySpendings = await _spendingService.GetMonthlySpendingsAsync(User);
                TotalSpendings = await _spendingService.GetTotalSpendingsAsync(User);
                Budget = await _spendingService.GetBudgetAsync(User);

                BudgetPercentage = (TotalSpendings/ Budget)*100 ;
                OverBudget = TotalSpendings - Budget;
            }

            return Page();
        }
    }
}
