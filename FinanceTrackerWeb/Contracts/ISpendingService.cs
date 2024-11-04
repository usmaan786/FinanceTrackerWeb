using System.Security.Claims;

namespace FinanceTrackerWeb.Contracts
{
    public interface ISpendingService
    {
        Task<Dictionary<string, double>> GetMonthlySpendingsAsync(ClaimsPrincipal user);
    }
}
