using System.Security.Claims;

namespace FinanceTrackerWeb.Contracts
{
    public interface ISpendingService
    {
        Task<Dictionary<string, double>> GetMonthlySpendingsAsync(ClaimsPrincipal user);
        Task<double> GetTotalSpendingsAsync(ClaimsPrincipal user);
        Task<double> GetBudgetAsync(ClaimsPrincipal user);
    }
}
