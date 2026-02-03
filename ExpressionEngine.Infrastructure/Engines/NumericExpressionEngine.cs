namespace ExpressionEngine.Infrastructure.Engines
{
    public class NumericExpressionEngine
    {
        internal static async Task<string> Calculate(string expression, double a, double b)
        {
            if (string.IsNullOrWhiteSpace(expression)) 
                throw new ArgumentException("Expression cannot be null or empty", nameof(expression));

            return await CalculateInternal(expression, a, b);
        }

        //private static async Task<string> CalculateInternal(string expression, double a, double b)
        //{
        //    //use ncalc
        //}
    }
}
