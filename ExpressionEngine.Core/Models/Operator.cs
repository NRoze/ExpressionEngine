using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Models
{
    public class Operator
    {
        public int Id { get; init; }
        public string Symbol { get; init; } = default!;
        public string Name { get; init; } = default!;
        public OperatorType Category { get; init; }
    }
}
