using _3_Calculator.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3_Calculator
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions opts) : base(opts)
        {
        }

        public DbSet<CalculationResult> CalculationResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalculationResult>()
                .HasIndex(x => new { x.Expression });

            base.OnModelCreating(modelBuilder);
        }
    }
}
