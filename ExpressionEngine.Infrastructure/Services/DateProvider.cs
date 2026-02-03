using ExpressionEngine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Infrastructure.Services
{
    public class DateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
