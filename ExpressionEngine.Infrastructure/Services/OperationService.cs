using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;

public sealed class OperationService : IOperationService
{
    private readonly IDateProvider _dateProvider;
    private readonly IRepository<Operation> _repo;
    private readonly IRepository<OperationHistory> _historyRepo;

    public OperationService(
        IDateProvider dateProvider,
        IRepository<Operation> repo, 
        IRepository<OperationHistory> historyRepo)
    {
        _dateProvider = dateProvider;
        _repo = repo;
        _historyRepo = historyRepo;
    }

    public async Task<CalculateResultDto> ExecuteAsync(CalculateRequestDto request)
    {
        var operation = await _repo.GetByIdAsync(request.OperationId) 
            ?? throw new ArgumentException("Operation not found", nameof(request.OperationId));
        string result;

        if (operation.OperationType == OperationType.Numeric)
        {
            if (double.TryParse(request.ValueA, out double a) &&
                double.TryParse(request.ValueB, out double b))
            {
                result = NumericExpressionEngine.Calculate(operation.Expression, a, b);

                if (string.IsNullOrEmpty(result))
                    throw new InvalidOperationException("Failed to calculate the expression.");

                await LogOperationUsage(operation.Id, request, result);

                return await BuildResultDTO(operation.Id, result);
            }
            
            throw new ArgumentException("Invalid numeric values", nameof(request));
        }

        result = StringExpressionEngine.Calculate(operation.Expression, request.ValueA, request.ValueB);

        await LogOperationUsage(operation.Id, request, result);

        return await BuildResultDTO(operation.Id, result);
    }

    private async Task LogOperationUsage(int operationId, CalculateRequestDto request, string result)
    {
        await _historyRepo.AddAsync(new OperationHistory
        {
            OperationId = operationId,
            A = request.ValueA,
            B = request.ValueB,
            Result = result,
            ExecutedAt = _dateProvider.UtcNow
        });
    }

    private async Task<CalculateResultDto> BuildResultDTO(int operationId, string result)
    {
        var dateNow = _dateProvider.UtcNow;
        var history = await _historyRepo.GetAllAsync();//TODO optimize to fetch only relevant records
        var thisMonthHistory = history
            .Where(h => 
                h.OperationId == operationId &&
                h.ExecutedAt.Year == dateNow.Year &&
                h.ExecutedAt.Month == dateNow.Month)
            .OrderByDescending(h => h.ExecutedAt)
            .ToList();
        var last3 = thisMonthHistory.Take(3);
        var count = thisMonthHistory.Count;

        return new CalculateResultDto(
            result,
            [.. last3.Select(h => new OperationHistoryDto(h.A, h.B, h.Result))],
            count);
    }
}
