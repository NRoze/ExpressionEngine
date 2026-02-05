using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Shared.DTOs
{
    public record CalculateRequestDto(Guid OperationId, string ValueA, string ValueB);
}
