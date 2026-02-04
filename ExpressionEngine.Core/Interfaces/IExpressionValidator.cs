using ExpressionEngine.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IExpressionValidator
    {
        Task<bool> ValidateAsync(string expression, OperationType type);
    }
}
