using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = null!;
        public TokenType Type { get; set; }
    }
}
