using ExpressionEngine.Shared.Enums;
using System.Text.Json.Serialization;

namespace ExpressionEngine.Shared.DTOs
{
    public record UpdateOperationDto(Guid OperationId, string Expression)
    {
        [JsonIgnore]
        public OperationType Type { get; set; }
    }
}
