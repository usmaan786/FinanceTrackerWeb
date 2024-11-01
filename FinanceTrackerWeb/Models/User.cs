using FinanceTrackerWeb.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerWeb.Models
{
    public class User : IdentityUser
    {

        /*[Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }*/

        public double? CurrentSpending { get; set; }
        public double? Budget { get; set; }
    }
}
