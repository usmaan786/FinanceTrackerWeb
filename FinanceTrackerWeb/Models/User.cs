using FinanceTrackerWeb.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerWeb.Models
{
    public class User : IdentityUser
    {

        public double CurrentSpending { get; set; } = 0;
        public double Budget { get; set; } = 0;
    }
}
