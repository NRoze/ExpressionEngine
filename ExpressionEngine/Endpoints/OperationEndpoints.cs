using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
            app.MapDelete("/api/operations/{operationId:guid}", DeleteOperation);
        }

        private async Task<IResult> CreateOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<CreateOperationDto> validator,
            IRepository<Operation> repo,
            CreateOperationDto request)
        {
            try
            {
                var result = await validator.ValidateAsync(request);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());

                var operation = new Operation(request.Name, request.Expression, request.Type);

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

                return Results.Problem($"Failed to create {request.Name} operation");
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
                var result = await validator.ValidateAsync(request);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());

                operation.UpdateExpression(request.Expression);

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

                return Results.Problem($"Failed to updtae {operation.Name} operation");
            }
        }

        private async Task<IResult> DeleteOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<DeleteOperationDto> validator,
            IRepository<Operation> repo,
            Guid operationId)
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

                return Results.Problem($"Failed to delete operation id {operationId} ");
            }
        }

        private async Task<IResult> GetOperations(IRepository<Operation> repo)
        {
            var operations = await repo.Query()
                .Select(o => new OperationDto(o.Id, o.Name))
                .ToListAsync();

            return Results.Ok(operations);
        }

        private async Task<IResult> CalculateOperation(
            ILogger<OperationEndpoints> logger,
            IValidator<CalculateRequestDto> validator,
            IOperationService operationService,
            CalculateRequestDto request)
        {
            try
            {
                var validationRresult = await validator.ValidateAsync(request);
                if (!validationRresult.IsValid)
                    return Results.ValidationProblem(validationRresult.ToDictionary());

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

                return Results.Problem($"Failed to calculate operation id {request.OperationId}");
            }
        }
    }
}
