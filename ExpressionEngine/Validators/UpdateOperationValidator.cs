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
                .NotNull();

            RuleFor(x => x.Expression)
                .NotEmpty()
                .WithMessage("Expression is required")
                .MaximumLength(500)
                .WithMessage("Expression cannot exceed 100 characters")
                .Must((dto, expr) => expressionValidator.Validate(expr, dto.Type))
                .WithMessage("Invalid expression format or unsupported tokens");
        }
    }
}
