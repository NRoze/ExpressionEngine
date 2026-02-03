using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Shared.DTOs
{
    public record CalculateRequest(int OperationId, string ValueA, string ValueB);
}
