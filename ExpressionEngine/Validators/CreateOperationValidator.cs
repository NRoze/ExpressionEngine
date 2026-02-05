using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Validators
{
    public class CreateOperationValidator : AbstractValidator<CreateOperationDto>
    {
        public CreateOperationValidator(IExpressionValidator expressionValidator)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Expression)
                .NotEmpty()
                .WithMessage("Expression is required")
                .MaximumLength(500)
                .WithMessage("Expression cannot exceed 500 characters")
                .MustAsync(async (dto, expr, ct) => // CancellationToken is ignored for simplicity in this demo
                    await expressionValidator.ValidateAsync(expr, dto.Type))
                .WithMessage("Invalid expression format or unsupported tokens");
        }
    }
}
