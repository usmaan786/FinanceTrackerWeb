using FinanceTrackerWeb.Data;
using FinanceTrackerWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using FinanceTrackerWeb.Contracts;

namespace FinanceTrackerWeb.Services
{
    public class SpendingService : ISpendingService
    {
        private readonly FinanceContext _context;
        private readonly UserManager<User> _userManager;

        public SpendingService(FinanceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Dictionary<string, double>> GetMonthlySpendingsAsync(ClaimsPrincipal user)
        {
            var currentUser_Id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var monthlySpendings = await _context.Spendings
                .Where(s => s.UserId == currentUser_Id)
                .GroupBy(s => new
                {
                    Year = s.TransactionDate.Year,
                    Month = s.TransactionDate.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalSpent = g.Sum(s => s.Spent)
                })
                .ToListAsync();

            // Convert to dictionary where the key is in "yyyy-MM" format
            return monthlySpendings.ToDictionary(
                m => $"{m.Year}-{m.Month:D2}", // Format as "yyyy-MM"
                m => m.TotalSpent);
        }
    }
}
