using ExpressionEngine.Infrastructure.Extensions;
using System.Text;

namespace ExpressionEngine.Infrastructure.Engines;

public static class StringExpressionEngine
{
    public static string Calculate(string expression, string a, string b)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("Expression cannot be null or empty", nameof(expression));

        var parser = new Parser(expression, a, b);
        var result = parser.ParseExpression();

        parser.EnsureEnd();

        return result;
    }

    private sealed class Parser
    {
        private readonly string _expr;
        private readonly string _a;
        private readonly string _b;
        private readonly bool _bIsInt;
        private readonly int _bInt;
        private int _pos;

        public Parser(string expr, string a, string b)
        {
            _pos = 0;
            _expr = expr.RemoveWhitespace();
            _a = a;
            _b = b;
            _bIsInt = int.TryParse(b, out _bInt); ;
        }

        public string ParseExpression()
        {
            var left = ParseTerm();

            while (true)
            {
                if (Match('+'))
                {
                    left += ParseTerm();
                }
                else if (Match('-'))
                {
                    left = left.RemoveSubstring(ParseTerm());
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        private string ParseTerm()
        {
            var left = ParseFactor();

            if (Match('*'))
            {
                if (!Match('B'))
                    throw new InvalidOperationException("After '*' only 'B' is allowed.");

                if (!_bIsInt)
                    throw new InvalidOperationException("B must be an integer to use '*'.");

                left = left.RepeatString(_bInt);
            }

            return left;
        }

        private string ParseFactor()
        {
            if (Match('('))
            {
                var value = ParseExpression();
                if (!Match(')'))
                    throw new InvalidOperationException("Missing closing parenthesis");
                return value;
            }

            return Match('A') ? _a 
                : Match('B') ? _b
                : throw new InvalidOperationException($"Unexpected token '{CurrentChar()}' at position {_pos}");
        }

        private bool Match(char c)
        {
            if (_pos < _expr.Length && _expr[_pos] == c)
            {
                _pos++;
                return true;
            }

            return false;
        }

        private char CurrentChar() => _pos < _expr.Length ? _expr[_pos] : '\0';

        public void EnsureEnd()
        {
            if (_pos != _expr.Length)
                throw new InvalidOperationException("Unexpected characters at end of expression");
        }
    }
}
