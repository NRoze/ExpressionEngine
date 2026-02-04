using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Validators
{
    public class UpdateOperationValidator : AbstractValidator<UpdateOperationDto>
    {
        public UpdateOperationValidator(IExpressionValidator expressionValidator)
        {
            RuleFor(x => x.OperationId)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Expression)
                .NotEmpty()
                .WithMessage("Expression is required")
                .MaximumLength(500)
                .WithMessage("Expression cannot exceed 100 characters")
                .MustAsync(async (dto, expr, ct) => // CancellationToken is ignored for simplicity in this demo
                    await expressionValidator.ValidateAsync(expr, dto.Type))
                .WithMessage("Invalid expression format or unsupported tokens");
        }
    }
}
