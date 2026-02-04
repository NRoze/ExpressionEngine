using ExpressionEngine.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IExpressionTokensValidator
    {
        Task<bool> ValidateAsync(string expression, OperationType type);
    }
}
