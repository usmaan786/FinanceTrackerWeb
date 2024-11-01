namespace FinanceTrackerWeb.Contracts
{
    public interface IUser
    {
        int Id { get; }
        string Name { get; }
        double? CurrentSpending { get; }
        double? Budget { get; }
    }
}
