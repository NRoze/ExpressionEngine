using ExpressionEngine.Core.Interfaces;

namespace ExpressionEngine.Infrastructure.Services
{
    public class DateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
