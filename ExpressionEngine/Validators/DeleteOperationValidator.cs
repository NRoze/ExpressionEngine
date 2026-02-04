using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Validators
{
    public class DeleteOperationValidator : AbstractValidator<DeleteOperationDto>
    {
        public DeleteOperationValidator(IExpressionValidator expressionValidator)
        {
            RuleFor(x => x.OperationId)
                .GreaterThanOrEqualTo(0);
        }
    }
}
