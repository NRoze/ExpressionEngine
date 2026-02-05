using ExpressionEngine.Infrastructure.Engines;

namespace ExpressionEngine.UnitTests.Handlers
{
    public class StringExpressionEngineTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Calculate_NullOrWhitespace_ThrowsArgumentException(string expression)
        {
            var ex = Assert.Throws<ArgumentException>(() => StringExpressionEngine.Calculate(expression, "a", "b"));
            Assert.Equal("expression", ex.ParamName);
        }

        [Fact]
        public void Calculate_Addition_ConcatenatesAAndB()
        {
            var result = StringExpressionEngine.Calculate("A+B", "foo", "bar");
            Assert.Equal("foobar", result);
        }

        [Fact]
        public void Calculate_Subtraction_RemovesAllOccurrencesOfBFromA()
        {
            var result = StringExpressionEngine.Calculate("A-B", "foobarbar", "bar");
            Assert.Equal("foo", result);
        }

        [Fact]
        public void Calculate_Multiply_RepeatsA_WhenBIsInteger()
        {
            var result = StringExpressionEngine.Calculate("A*B", "x", "3");
            Assert.Equal("xxx", result);
        }

        [Fact]
        public void Calculate_Multiply_InvalidTargetAfterAsterisk_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => StringExpressionEngine.Calculate("A*X", "x", "3"));
            Assert.Equal("After '*' only 'B' is allowed.", ex.Message);
        }

        [Fact]
        public void Calculate_Multiply_BNotInteger_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => StringExpressionEngine.Calculate("A*B", "x", "notint"));
            Assert.Equal("B must be an integer to use '*'.", ex.Message);
        }

        [Fact]
        public void Calculate_ParenthesesAndOperators_ProcessCorrectly()
        {
            // (A+B) -> "hellolo", then -B removes all "lo" occurrences -> "hel"
            var result = StringExpressionEngine.Calculate("(A+B)-B", "hello", "lo");
            Assert.Equal("hel", result);
        }

        [Fact]
        public void Calculate_UnexpectedToken_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => StringExpressionEngine.Calculate("C", "a", "b"));
            Assert.Contains("Unexpected token 'C'", ex.Message);
        }

        [Fact]
        public void Calculate_TrailingCharacters_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => StringExpressionEngine.Calculate("A+B#", "a", "b"));
            Assert.Equal("Unexpected characters at end of expression", ex.Message);
        }
    }
}
