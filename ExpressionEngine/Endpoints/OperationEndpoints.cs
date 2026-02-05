using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.DTOs;
using FluentValidation;

namespace ExpressionEngine.Api.Endpoints
{
    public class OperationEndpoints : IEndpointDefinition
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("/api/calculate", CalculateOperation);
            app.MapGet("/api/operations", GetOperations);
            app.MapPost("/api/operations", CreateOperation);
            app.MapPut("/api/operations", UpdateOperation);
            app.MapDelete("/api/operations/{operationId}", DeleteOperation);
        }

        private async Task<IResult> CreateOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<CreateOperationDto> validator,
            IRepository<Operation> repo,
            CreateOperationDto request)
        {
            try
            {
                if (!(await validator.ValidateAsync(request)).IsValid)
                    return Results.BadRequest("Invalid request");

                var operation = new Operation
                {
                    Name = request.Name,
                    Expression = request.Expression,
                    OperationType = request.Type
                };

                await repo.AddAsync(operation);

                return Results.Created($"/api/operations/{operation.Id}", new OperationDto(operation.Id, operation.Name));
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error creating operation {OperationName}: {OperationExpression}",
                    request.Name,
                    request.Expression);

                return Results.BadRequest($"Failed to create {request.Name} operation");
            }

        }

        private async Task<IResult> UpdateOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<UpdateOperationDto> validator,
            IRepository<Operation> repo,
            UpdateOperationDto request)
        {
            var operation = await repo.GetByIdAsync(request.OperationId);

            if (operation is null)
                return Results.NotFound($"Operation with ID {request.OperationId} not found");

            try
            {
                request.Type = operation.OperationType;
                if (!(await validator.ValidateAsync(request)).IsValid)
                    return Results.BadRequest("Invalid request");

                operation.Expression = request.Expression;

                await repo.UpdateAsync(operation);

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error updating operation {OperationName}: {OperationExpression}",
                    operation.Name,
                    request.Expression);

                return Results.BadRequest($"Failed to updtae {operation.Name} operation");
            }
        }

        private async Task<IResult> DeleteOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<DeleteOperationDto> validator,
            IRepository<Operation> repo,
            int operationId)
        {
            try
            {
                await repo.DeleteAsync(operationId);

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error deleting operation id {OperationId}",
                    operationId);

                return Results.BadRequest($"Failed to delete operation id {operationId} ");
            }
        }

        private async Task<IResult> GetOperations(IRepository<Operation> repo)
        {
            var operations = await repo.GetAllAsync();

            if (operations is null) return Results.Empty;

            return Results.Ok(operations.Select(o => new OperationDto(o.Id, o.Name)).ToList());
        }

        private async Task<IResult> CalculateOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<CalculateRequestDto> validator,
            IOperationService operationService,
            CalculateRequestDto request)
        {
            try
            {
                if (!(await validator.ValidateAsync(request)).IsValid)
                    return Results.BadRequest("Invalid request");

                var result = await operationService.ExecuteAsync(request);

                return Results.Ok(result);

            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error calculating operation {OperationId} with A={A} and B={B}", 
                    request.OperationId,
                    request.ValueA,
                    request.ValueB);

                return Results.BadRequest(ex.Message);
            }
        }
    }
}
