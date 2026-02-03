using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.DTOs;
using System.ClientModel.Primitives;

public sealed class OperationService : IOperationService
{
    private readonly IRepository<Operation> _repo;

    public OperationService(IRepository<Operation> repo)
    {
        _repo = repo;
    }

    public async Task<CalculateResultDto> ExecuteAsync(CalculateRequest request)
    {
        var operation = await _repo.GetByIdAsync(request.OperationId);

        if (operation is null)
            throw new ArgumentException("Operation not found", nameof(request.OperationId));

        double a, b;

        if (double.TryParse(request.ValueA, out a) &&
            double.TryParse(request.ValueB, out b))
        {
            var result = NumericExpressionEngine.Calculate(operation.Expression, a, b);
        }
        //temp return string exec
        return new CalculateResultDto("4", new List<OperationHistoryDto>(), 0);
    }
}
