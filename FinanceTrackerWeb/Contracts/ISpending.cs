namespace FinanceTrackerWeb.Contracts
{
    public interface ISpending
    {
        string Item { get; }
        double Spent { get; }
        DateTime TransactionDate { get; }
    }
}
