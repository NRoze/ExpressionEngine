using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.Core.Models
{
    public class Operation
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Expression { get; private set; } = default!;
        public OperationType OperationType { get; private set; }
        private Operation() { }

        public Operation(string name, string expression, OperationType type)
        {
            Id = Guid.CreateVersion7();
            Name = name;
            Expression = expression;
            OperationType = type;
        }
        public Operation(Guid id, string name, string expression, OperationType type)
        : this(name, expression, type)
        {
            Id = id;
        }

        public void UpdateExpression(string newExpression)
        {
            Expression = newExpression;
        }
    }
}
