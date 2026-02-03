using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Models
{
    public class OperationHistory
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = default!;
        public string A { get; set; } = default!;
        public string B { get; set; } = default!;
        public string Result { get; set; } = default!;
        public DateTime ExecutedAt { get; set; }
    }
}
