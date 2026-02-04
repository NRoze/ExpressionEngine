using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Infrastructure.Validators
{
    public sealed class ExpressionTokensValidator : IExpressionTokensValidator
    {
        private readonly IRepository<Token> _tokenRepo;
        private readonly IRepository<Operator> _operatorRepo;

        public ExpressionTokensValidator(IRepository<Token> tokenRepo, IRepository<Operator> operatorRepo)
        {
            _tokenRepo = tokenRepo;
            _operatorRepo = operatorRepo;
        }

        public async Task<bool> ValidateAsync(string expression, OperationType type)
        {
            var tokens = await _tokenRepo.GetAllAsync();
            var operators = await _operatorRepo.GetAllAsync();

            try
            {
                var tokenList = Tokenize(expression, tokens);

                return ValidateTokens(tokenList, tokens, operators, type);
            }
            catch (FormatException) { }

            return false;
        }

        private IReadOnlyList<string> Tokenize(string expression, IReadOnlyList<Token> tokenDefinitions)
        {
            var tokens = new List<string>();

            int i = 0;
            while (i < expression.Length)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    i++;
                    continue;
                }

                // Try match the longest token from tokenDefinitions
                string? matched = null;

                foreach (var token in tokenDefinitions.OrderByDescending(t => t.Symbol.Length))
                {
                    if (expression.Length - i >= token.Symbol.Length &&
                        string.Compare(expression, i, token.Symbol, 0, token.Symbol.Length, true) == 0)
                    {
                        matched = token.Symbol;
                        break;
                    }
                }

                if (matched == null)
                    throw new FormatException($"Unknown token at position {i}");

                tokens.Add(matched);
                i += matched.Length;
            }

            return tokens;
        }
        private bool ValidateTokens(
            IReadOnlyList<string> tokens,
            IReadOnlyList<Token> tokenDefinitions,
            IReadOnlyList<Operator> operatorDefinitions,
            OperationType operationType)
        {
            foreach (var token in tokens)
            {
                var def = tokenDefinitions.FirstOrDefault(t =>
                    string.Equals(t.Symbol, token, StringComparison.OrdinalIgnoreCase));

                if (def == null)
                    return false;

                if (def.Type == TokenType.Operator ||
                    def.Type == TokenType.Function)
                {
                    var op = operatorDefinitions.First(o => o.Symbol == def.Symbol);

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
