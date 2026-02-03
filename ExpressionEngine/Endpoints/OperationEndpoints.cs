using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.DTOs;

namespace ExpressionEngine.Api.Endpoints
{
    public class OperationEndpoints : IEndpointDefinition
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("/api/calculate", CalculateOperation);
            app.MapGet("/api/operations", GetOperations);
            app.MapPost("/api/operations", CreateOperation);
        }

        private async Task<IResult> CreateOperation(
            IRepository<Operation> repo,
            CreateOperationDto request)
        {
            //TODO expression validation
            var operation = new Operation
            {
                Name = request.Name,
                Expression = request.Expression,
                OperationType = request.Type
            };

            await repo.AddAsync(operation);

            return Results.Created($"/api/operations/{operation.Id}", new OperationDto(operation.Id, operation.Name));
        }

        private async Task<IResult> GetOperations(IRepository<Operation> repo)
        {
            var operations = await repo.GetAllAsync();

            if (operations is null) return Results.Empty;

            return Results.Ok(operations.Select(o => new OperationDto(o.Id, o.Name)).ToList());
        }

        private async Task<IResult> CalculateOperation(
            IOperationService operationService,
            CalculateRequestDto request)
        {
            try
            {
                var result = await operationService.ExecuteAsync(request);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
