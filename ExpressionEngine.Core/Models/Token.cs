using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Models
{
    public class Token
    {
        public int Id { get; init; }
        public string Symbol { get; init; } = default!;
        public TokenType Type { get; init; }
    }
}
