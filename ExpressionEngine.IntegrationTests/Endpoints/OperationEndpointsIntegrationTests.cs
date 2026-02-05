using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;

namespace ExpressionEngine.IntegrationTests.Endpoints;

public class OperationEndpointsIntegrationTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OperationEndpointsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOperations_WhenEmpty_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/operations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateOperation_WithInvalidModel_ReturnsBadRequest()
    {
        var dto = new CreateOperationDto("", "A + B", OperationType.Numeric);

        var response = await _client.PostAsJsonAsync("/api/operations", dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateOperation_WithValidModel_ReturnsCreated()
    {
        var name = $"Op_{Guid.NewGuid()}";
        var dto = new CreateOperationDto(name, "A + B", OperationType.Numeric);

        var response = await _client.PostAsJsonAsync("/api/operations", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<OperationDto>();

        body.Should().NotBeNull();
        body!.Name.Should().Be(name);
        body.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateOperation_NotFound_ReturnsNotFound()
    {
        var dto = new UpdateOperationDto(999, "new expr");

        var response = await _client.PutAsJsonAsync("/api/operations", dto);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteOperation_ReturnsNoContent()
    {
        var name = $"Op_{Guid.NewGuid()}";
        var create = new CreateOperationDto(name, "A + B", OperationType.Numeric);
        var createResp = await _client.PostAsJsonAsync("/api/operations", create);
        var created = await createResp.Content.ReadFromJsonAsync<OperationDto>();

        var response = await _client.DeleteAsync($"/api/operations/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CalculateOperation_InvalidModel_ReturnsBadRequest()
    {
        var dto = new CalculateRequestDto(1, "", "");

        var response = await _client.PostAsJsonAsync("/api/calculate", dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CalculateOperation_ValidModel_ReturnsOk()
    {
        var name = $"Op_{Guid.NewGuid()}";
        var create = new CreateOperationDto(name, "A + B", OperationType.Numeric);
        var createResp = await _client.PostAsJsonAsync("/api/operations", create);
        var created = await createResp.Content.ReadFromJsonAsync<OperationDto>();

        var dto = new CalculateRequestDto(created!.Id, "1", "2");

        var response = await _client.PostAsJsonAsync("/api/calculate", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
