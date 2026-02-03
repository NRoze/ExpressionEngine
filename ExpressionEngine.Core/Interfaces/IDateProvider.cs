using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IDateProvider
    {
        DateTime UtcNow { get; }
    }
}
