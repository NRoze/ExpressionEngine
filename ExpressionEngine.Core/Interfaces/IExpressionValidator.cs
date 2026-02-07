using ExpressionEngine.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IExpressionValidator
    {
        bool Validate(string expression, OperationType type);
    }
}
