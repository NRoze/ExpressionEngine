using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Engines;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;
using Microsoft.EntityFrameworkCore;

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

    private async Task LogOperationUsage(Guid operationId, CalculateRequestDto request, string result)
    {
        await _historyRepo.AddAsync(new OperationHistory(operationId, request.ValueA, request.ValueB, result, _dateProvider.UtcNow));
    }

    private async Task<CalculateResultDto> BuildResultDTO(Guid operationId, string result)
    {
        var dateNow = _dateProvider.UtcNow;
        var start = new DateTime(dateNow.Year, dateNow.Month, 1);
        var historyQuery = _historyRepo.Query()
            .Where(h => h.OperationId == operationId)
            .Where(h => h.ExecutedAt >= start && h.ExecutedAt <= dateNow)
            .OrderByDescending(h => h.ExecutedAt)
            .Select(h => new OperationHistoryDto(h.A, h.B, h.Result));

        List<OperationHistoryDto> last3List;
        int count;

        // If the IQueryable supports EF Core async enumeration, use async methods.
        // Otherwise fall back to synchronous enumeration (works for mocks/AsQueryable()).
        if (historyQuery is IAsyncEnumerable<OperationHistory>)
        {
            last3List = await historyQuery.Take(3).ToListAsync();
            count = await historyQuery.CountAsync();
        }
        else
        {
            var all = historyQuery.ToList();
            last3List = all.Take(3).ToList();
            count = all.Count;
        }

        return new CalculateResultDto(result, last3List, count);
    }
}
