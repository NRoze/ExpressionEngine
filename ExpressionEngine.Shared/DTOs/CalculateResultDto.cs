using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Shared.DTOs
{
    public record CalculateResultDto(string Result, List<OperationHistoryDto> LastOperations, int TotalCount);
}
