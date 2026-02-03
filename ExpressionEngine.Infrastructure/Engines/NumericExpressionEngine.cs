using NCalc;

namespace ExpressionEngine.Infrastructure.Engines
{
    public class NumericExpressionEngine
    {
        internal static string Calculate(string expression, double a, double b)
        {
            if (string.IsNullOrWhiteSpace(expression)) 
                throw new ArgumentException("Expression cannot be null or empty", nameof(expression));

            return CalculateInternal(expression, a, b);
        }

        private static string CalculateInternal(string expression, double a, double b)
        {
            var e = new Expression(expression);

            e.Parameters["A"] = a;
            e.Parameters["B"] = b;

            e.EvaluateFunction += (name, args) =>
            {
                if (name.Equals("min", StringComparison.OrdinalIgnoreCase))
                {
                    args.Result = Math.Min(Convert.ToDouble(args.Parameters[0].Evaluate()), Convert.ToDouble(args.Parameters[1].Evaluate()));
                }
                else if (name.Equals("max", StringComparison.OrdinalIgnoreCase))
                {
                    args.Result = Math.Max(Convert.ToDouble(args.Parameters[0].Evaluate()), Convert.ToDouble(args.Parameters[1].Evaluate()));
                }
            };

            var result = e.Evaluate();

            return result?.ToString() ?? string.Empty;
        }
    }
}
