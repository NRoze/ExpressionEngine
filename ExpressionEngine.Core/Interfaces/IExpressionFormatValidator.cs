using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IExpressionFormatValidator
    {
        bool Validate(string expression, OperationType operationType);
    }
}
