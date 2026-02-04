using ExpressionEngine.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IExpressionFormatValidator
    {
        bool Validate(string expression, OperationType operationType);
    }
}
