using Microsoft.EntityFrameworkCore;
using FinanceTrackerWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinanceTrackerWeb.Data
{
    public class FinanceContext : IdentityDbContext<User>
    {
        public DbSet<Spending> Spendings { get; set; }

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Spending>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
