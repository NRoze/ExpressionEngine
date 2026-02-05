using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Models
{
    public class OperationHistory
    {
        public Guid Id { get; private set; }
        public Guid OperationId { get; init; }
        public string A { get; init; } = default!;
        public string B { get; init; } = default!;
        public string Result { get; init; } = default!;
        public DateTime ExecutedAt { get; init; }

        private OperationHistory() { }

        public OperationHistory(Guid operationId, string a, string b, string result, DateTime executedAt)
        {
            Id = Guid.CreateVersion7();
            OperationId = operationId;
            A = a;
            B = b;
            Result = result;
            ExecutedAt = executedAt;
        }
    }
}
