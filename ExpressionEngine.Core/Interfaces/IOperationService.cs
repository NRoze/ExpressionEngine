using ExpressionEngine.Shared.DTOs;

namespace ExpressionEngine.Core.Interfaces
{
    public interface IOperationService
    {
        Task<CalculateResultDto> ExecuteAsync(CalculateRequest request);
    }
}
