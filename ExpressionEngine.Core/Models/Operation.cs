using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEngine.Core.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Expression { get; set; } = default!;
    }
}
