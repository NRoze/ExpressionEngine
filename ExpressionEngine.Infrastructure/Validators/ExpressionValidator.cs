using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Infrastructure.Validators
{
    public sealed class ExpressionValidator : IExpressionValidator
    {
        private readonly IExpressionTokensValidator _tokensValidator;
        private readonly IExpressionFormatValidator _formatValidator;
        public ExpressionValidator(
            IExpressionTokensValidator tokensValidator,
            IExpressionFormatValidator formatValidator)
        {
            _tokensValidator = tokensValidator;
            _formatValidator = formatValidator;
        }

        public bool Validate(string expression, OperationType type)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return false;

            if (!_tokensValidator.Validate(expression, type))
                return false;

            if (!_formatValidator.Validate(expression, type))
                return false;

            return true;
        }
    }
}
