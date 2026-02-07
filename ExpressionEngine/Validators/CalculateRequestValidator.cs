using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Validators
{
    public class CalculateRequestValidator : AbstractValidator<CalculateRequestDto>
    {
        private const int MaxRepeatCount = 100;
        private const int MaxOutputLength = 100_000;

        public CalculateRequestValidator()
        {
            RuleFor(x => x.OperationId)
                .NotEmpty();

            RuleFor(x => x.ValueA)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ValueB)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x)
                .Must(ValidStringOperation)
                .WithMessage("Invalid string operation inputs.");
        }

        private bool ValidStringOperation(CalculateRequestDto dto)
        {
            // If B isn't numeric, no need to validate repeat count here
            if (!int.TryParse(dto.ValueB, out int repeat))
            {
                return true;
            }

            return repeat >= 0 && repeat <= MaxRepeatCount &&
                   (dto.ValueA.Length * repeat) <= MaxOutputLength;
        }
    }
}
