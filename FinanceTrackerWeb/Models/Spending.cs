using FinanceTrackerWeb.Contracts;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerWeb.Models
{
    public class Spending : ISpending
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public double Spent { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }//optional property
    }
}
