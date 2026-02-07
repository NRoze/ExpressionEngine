using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Infrastructure.Validators
{
    public sealed class ExpressionFormatValidator : IExpressionFormatValidator
    {
        private readonly List<(double a, double b)> NumericCandidates =
        [
            (2, 3),
            (10, 5),
            (7, 2),
            (4, 1)
        ];

        private readonly List<(string a, string b)> StringCandidates =
        [
            ("hello ", "world"),
            ("x", "3"),
            ("abc", "2"),
            ("A", "B")
        ];

        private readonly List<(string a, string b)> StringCandidatesMultiplication =
        [
            ("hello ", "2"),
            ("x", "3"),
            ("abc", "4"),
            ("A", "12")
        ];

        public bool Validate(string expression, OperationType operationType)
        {
            return operationType switch
            {
                OperationType.Numeric => ValidateWithNumericEngine(expression),
                OperationType.String =>
                    expression.Contains('*') ? ValidateWithMultiplicationStringEngine(expression)
                                            : ValidateWithStringEngine(expression),
                _ => false
            };
        }

        private bool ValidateWithNumericEngine(string expression)
        {
            foreach (var (a, b) in NumericCandidates)
            {
                try
                {
                    NumericExpressionEngine.Calculate(expression, a, b);
                    return true;
                }
                catch { } // ignore and try next candidate
            }

            return false;
        }

        private bool ValidateWithStringEngine(string expression)
        {
            foreach (var (a, b) in StringCandidates)
            {
                try
                {
                    StringExpressionEngine.Calculate(expression, a, b);
                    return true;
                }
                catch { } // ignore and try next candidate
            }

            return false;
        }

        private bool ValidateWithMultiplicationStringEngine(string expression)
        {
            foreach (var (a, b) in StringCandidatesMultiplication)
            {
                try
                {
                    StringExpressionEngine.Calculate(expression, a, b);
                    return true;
                }
                catch { } // ignore and try next candidate
            }

            return false;
        }

    }
}
