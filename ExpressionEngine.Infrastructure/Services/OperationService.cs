using ExpressionEngine.Core.Enums;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.DTOs;
using System.ClientModel.Primitives;

public sealed class OperationService : IOperationService
{
    private readonly IRepository<Operation> _repo;
    private readonly IRepository<OperationHistory> _historyRepo;

    public OperationService(IRepository<Operation> repo, IRepository<OperationHistory> historyRepo)
    {
        _repo = repo;
        _historyRepo = historyRepo;
    }

    public async Task<CalculateResultDto> ExecuteAsync(CalculateRequest request)
    {
        var operation = await _repo.GetByIdAsync(request.OperationId);

        if (operation is null)
            throw new ArgumentException("Operation not found", nameof(request.OperationId));

        double a, b;
        string result;

        if (operation.OperationType == OperationType.Numeric)
        {
            if (double.TryParse(request.ValueA, out a) &&
                double.TryParse(request.ValueB, out b))
            {
                result = NumericExpressionEngine.Calculate(operation.Expression, a, b);

                if (string.IsNullOrEmpty(result))
                    throw new InvalidOperationException("Failed to calculate the expression.");

                return await BuildResultDTO(request.OperationId, result);
            }
            
            throw new ArgumentException("Invalid numeric values", nameof(request));
        }

        result = StringExpressionEngine.Calculate(operation.Expression, request.ValueA, request.ValueB);

        return await BuildResultDTO(request.OperationId, result);
    }

    private async Task<CalculateResultDto> BuildResultDTO(int operationId, string result)
    {
        var history = await _historyRepo.GetAllAsync();
        var sameOperationHistory = history.Where(h => h.Id == operationId).ToList();
        var thisMonthHistory = sameOperationHistory.Where(
            h => 
                h.ExecutedAt.Year == DateTime.UtcNow.Year &&
                h.ExecutedAt.Month == DateTime.UtcNow.Month).ToList();
        var count = sameOperationHistory.Count;

        return new CalculateResultDto(
            result, 
            thisMonthHistory.Select(h => new OperationHistoryDto(h.A, h.B, h.Result)).ToList(), 
            count);
    }
}
