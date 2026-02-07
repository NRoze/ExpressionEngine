using ExpressionEngine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpressionEngine.UnitTests.Helpers
{
    internal sealed class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<OperationHistory> OperationHistories { get; set; }
    }
}
