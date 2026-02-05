using System.Globalization;
using System.Reflection;
using ExpressionEngine.Infrastructure.Engines;

namespace ExpressionEngine.UnitTests.Handlers
{
    public class NumericExpressionEngineTests
    {
        private static MethodInfo GetCalculateMethod() =>
            typeof(NumericExpressionEngine).GetMethod("Calculate", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            ?? throw new InvalidOperationException("Could not find Calculate method on NumericExpressionEngine.");

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Calculate_NullOrWhitespace_ThrowsArgumentException(string expression)
        {
            var mi = GetCalculateMethod();

            var ex = Assert.Throws<TargetInvocationException>(() =>
                mi.Invoke(null, new object[] { expression, 1.0, 2.0 }));

            Assert.IsType<ArgumentException>(ex.InnerException);
            var argEx = (ArgumentException)ex.InnerException!;
            Assert.Equal("expression", argEx.ParamName);
        }

        [Fact]
        public void Calculate_Addition_ReturnsSum()
        {
            var mi = GetCalculateMethod();

            var result = mi.Invoke(null, new object[] { "A + B", 1.5, 2.25 }) as string;

            Assert.NotNull(result);
            var value = double.Parse(result!, CultureInfo.InvariantCulture);
            Assert.Equal(3.75, value, 5);
        }

        [Theory]
        [InlineData("min(A,B)", 5.0, 3.0, 3.0)]
        [InlineData("max(A,B)", 5.0, 3.0, 5.0)]
        [InlineData("MiN(A,B)", -1.0, -5.0, -5.0)]
        [InlineData("MaX(A,B)", -1.0, -5.0, -1.0)]
        public void Calculate_MinMaxFunctions_ReturnExpected(string expression, double a, double b, double expected)
        {
            var mi = GetCalculateMethod();

            var result = mi.Invoke(null, new object[] { expression, a, b }) as string;

            Assert.NotNull(result);
            var value = double.Parse(result!, CultureInfo.InvariantCulture);
            Assert.Equal(expected, value, 10);
        }

        [Fact]
        public void Calculate_ComplexExpression_ComputesCorrectly()
        {
            var mi = GetCalculateMethod();

            // max(4,6)=6 -> 6*2 = 12 ; min(4,6)=4 -> 4/2 = 2 ; total = 14
            var result = mi.Invoke(null, new object[] { "max(A,B) * 2 + min(A,B) / 2", 4.0, 6.0 }) as string;

            Assert.NotNull(result);
            var value = double.Parse(result!, CultureInfo.InvariantCulture);
            Assert.Equal(14.0, value, 10);
        }
    }
}
