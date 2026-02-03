using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Data;
using ExpressionEngine.Shared.DTOs;
using Microsoft.EntityFrameworkCore.Design;

namespace ExpressionEngine.Api.Endpoints
{
    public class OperationEndpoints : IEndpointDefinition
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapGet("/api/operations", GetOperations);
            app.MapPost("/api/calculate", CalculateOperation);
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
