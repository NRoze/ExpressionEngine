using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace ExpressionEngine.Infrastructure.Validators
{
    public sealed class ExpressionTokensValidator : IExpressionTokensValidator
    {
        private readonly ILogger<ExpressionTokensValidator> _logger;
        private readonly IReadOnlyList<Token> _tokensLongToShort;
        private readonly Dictionary<string, Token> _tokens;
        private readonly Dictionary<string, Operator> _operators;

        public ExpressionTokensValidator(
            ILogger<ExpressionTokensValidator> logger,
            IRepository<Token> tokenRepo, 
            IRepository<Operator> operatorRepo)
        {
            _logger = logger;

            var tokens = tokenRepo.Query().ToArray();

            _tokensLongToShort = [.. tokens.OrderByDescending(t => t.Symbol.Length)];
            _tokens = tokens.ToDictionary(t => t.Symbol, StringComparer.OrdinalIgnoreCase);

            _operators = operatorRepo.Query()
                .ToDictionary(o => o.Symbol, StringComparer.OrdinalIgnoreCase);
        }

        public bool Validate(string expression, OperationType type)
        {
            try
            {
                if (!TryTokenize(expression, out var tokenList))
                    return false;

                return ValidateTokens(tokenList, type);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error validating expression tokens for expression: {Expression}", expression);
            }

            return false;
        }

        private bool TryTokenize(ReadOnlySpan<char> expression, out List<string> tokenList)
        {
            tokenList = new List<string>(expression.Length);
            int i = 0;

            while (i < expression.Length)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    i++;
                    continue;
                }

                string? matched = null;

                foreach (var token in _tokensLongToShort)
                {
                    if (expression.Length - i >= token.Symbol.Length &&
                        expression.Slice(i, token.Symbol.Length)
                                    .Equals(token.Symbol, StringComparison.OrdinalIgnoreCase))
                    {
                        matched = token.Symbol;
                        break;
                    }
                }

                if (matched == null)
                    return false;

                tokenList.Add(matched);
                i += matched.Length;
            }

            return true;
        }

        private bool ValidateTokens(
            IReadOnlyList<string> tokens,
            OperationType operationType)
        {
            foreach (var token in tokens)
            {
                if (!_tokens.TryGetValue(token, out var def))
                    return false;

                if (def.Type == TokenType.Operator ||
                    def.Type == TokenType.Function)
                {
                    if (!_operators.TryGetValue(def.Symbol, out var op))
                        return false;

                    if (operationType == OperationType.String &&
                        op.Category == OperatorType.NumericOnly)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
