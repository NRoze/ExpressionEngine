using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Shared.DTOs
{
    public record CreateOperationDto(string Name, string Expression, OperationType Type);
}
