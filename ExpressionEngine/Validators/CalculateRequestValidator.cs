using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Validators
{
    public class CalculateRequestValidator : AbstractValidator<CalculateRequestDto>
    {
        public CalculateRequestValidator()
        {
            RuleFor(x => x.OperationId)
                .NotEmpty();

            RuleFor(x => x.ValueA)
                .NotEmpty();

            RuleFor(x => x.ValueB)
                .NotEmpty();
        }
    }
}
