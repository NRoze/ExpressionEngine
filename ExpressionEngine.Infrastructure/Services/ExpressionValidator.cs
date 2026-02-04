using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Infrastructure.Services
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

        public async Task<bool> ValidateAsync(string expression, OperationType type)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return false;

            if (!await _tokensValidator.ValidateAsync(expression, type))
                return false;

            if (!_formatValidator.Validate(expression, type))
                return false;

            return true;
        }
    }
}
