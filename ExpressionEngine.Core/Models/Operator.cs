using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Models
{
    public class Operator
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public OperatorType Category { get; set; }
    }
}
