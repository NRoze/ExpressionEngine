using ExpressionEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Models
{
    public class Operator
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public OperationCategory Category { get; set; }
    }
}
