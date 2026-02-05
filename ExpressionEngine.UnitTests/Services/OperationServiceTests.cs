using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.DTOs;
using ExpressionEngine.Shared.Enums;
using Moq;

namespace ExpressionEngine.UnitTests.Services
{
    public class OperationServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_NumericOperation_ParsesValues_LogsAndReturnsResultAndHistory()
        {
            // Arrange
            var name = $"Op_{Guid.NewGuid()}";
            var now = new DateTime(2026, 2, 5, 12, 0, 0, DateTimeKind.Utc);
            var dateProviderMock = new Mock<IDateProvider>();
            dateProviderMock.SetupGet(d => d.UtcNow).Returns(now);

            var operation = new Operation(name, "A + B", OperationType.Numeric);

            var repoMock = new Mock<IRepository<Operation>>();
            repoMock.Setup(r => r.GetByIdAsync(operation.Id)).ReturnsAsync(operation);

            OperationHistory? addedHistory = null;
            var historyRepoMock = new Mock<IRepository<OperationHistory>>();
            historyRepoMock
                .Setup(h => h.AddAsync(It.IsAny<OperationHistory>()))
                .Callback<OperationHistory>(h => addedHistory = h)
                .Returns(Task.CompletedTask);

            // Prepare history returned by GetAllAsync - 4 items so last3 are top 3 newest
            var histories = new List<OperationHistory>
            {
                new OperationHistory(operation.Id, "x", "y", "r1", now.AddMinutes(-3)),
                new OperationHistory(operation.Id, "x", "y", "r2", now.AddMinutes(-2)),
                new OperationHistory(operation.Id, "x", "y", "r3", now.AddMinutes(-1)),
                new OperationHistory(operation.Id, "x", "y", "r4", now) // newest
            };
            // Return unsorted list to ensure service does ordering itself
            historyRepoMock.Setup(h => h.GetAllAsync()).ReturnsAsync(histories);

            var service = new OperationService(dateProviderMock.Object, repoMock.Object, historyRepoMock.Object);

            var request = new CalculateRequestDto(operation.Id, "1", "2");

            // Act
            var resultDto = await service.ExecuteAsync(request);

            // Assert
            Assert.NotNull(resultDto);
            Assert.Equal("3", resultDto.Result); // 1 + 2 == 3
            Assert.Equal(3, resultDto.LastOperations.Count); // last 3
            Assert.Equal(4, resultDto.TotalCount); // total matching this month

            // Verify history was logged with expected values
            Assert.NotNull(addedHistory);
            Assert.Equal(operation.Id, addedHistory.OperationId);
            Assert.Equal("1", addedHistory.A);
            Assert.Equal("2", addedHistory.B);
            Assert.Equal("3", addedHistory.Result);
            Assert.Equal(now, addedHistory.ExecutedAt);

            historyRepoMock.Verify(h => h.AddAsync(It.IsAny<OperationHistory>()), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(operation.Id), Times.Once);
            historyRepoMock.Verify(h => h.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_StringOperation_ConcatenatesAndLogsHistory()
        {
            // Arrange
            var now = new DateTime(2026, 2, 5, 12, 0, 0, DateTimeKind.Utc);
            var name = $"Op_{Guid.NewGuid()}";

            var dateProviderMock = new Mock<IDateProvider>();
            dateProviderMock.SetupGet(d => d.UtcNow).Returns(now);

            var operation = new Operation(name, "A+B", OperationType.String);

            var repoMock = new Mock<IRepository<Operation>>();
            repoMock.Setup(r => r.GetByIdAsync(operation.Id)).ReturnsAsync(operation);

            OperationHistory? addedHistory = null;
            var historyRepoMock = new Mock<IRepository<OperationHistory>>();
            historyRepoMock
                .Setup(h => h.AddAsync(It.IsAny<OperationHistory>()))
                .Callback<OperationHistory>(h => addedHistory = h)
                .Returns(Task.CompletedTask);

            // Return an empty list for existing history
            historyRepoMock.Setup(h => h.GetAllAsync()).ReturnsAsync(new List<OperationHistory>());

            var service = new OperationService(dateProviderMock.Object, repoMock.Object, historyRepoMock.Object);

            var request = new CalculateRequestDto(operation.Id, "foo", "bar");

            // Act
            var resultDto = await service.ExecuteAsync(request);

            // Assert
            Assert.NotNull(resultDto);
            Assert.Equal("foobar", resultDto.Result);
            Assert.Empty(resultDto.LastOperations);
            Assert.Equal(0, resultDto.TotalCount);

            Assert.NotNull(addedHistory);
            Assert.Equal(operation.Id, addedHistory.OperationId);
            Assert.Equal("foo", addedHistory.A);
            Assert.Equal("bar", addedHistory.B);
            Assert.Equal("foobar", addedHistory.Result);
            Assert.Equal(now, addedHistory.ExecutedAt);

            historyRepoMock.Verify(h => h.AddAsync(It.IsAny<OperationHistory>()), Times.Once);
            repoMock.Verify(r => r.GetByIdAsync(operation.Id), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_OperationNotFound_ThrowsArgumentException()
        {
            // Arrange
            var dateProviderMock = new Mock<IDateProvider>();
            dateProviderMock.SetupGet(d => d.UtcNow).Returns(DateTime.UtcNow);

            var repoMock = new Mock<IRepository<Operation>>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Operation?)null);

            var historyRepoMock = new Mock<IRepository<OperationHistory>>();

            var service = new OperationService(dateProviderMock.Object, repoMock.Object, historyRepoMock.Object);

            var request = new CalculateRequestDto(Guid.NewGuid(), "a", "b");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecuteAsync(request));
            Assert.Equal("Operation not found (Parameter 'OperationId')", ex.Message);
            Assert.Equal("OperationId", ex.ParamName);
        }

        [Fact]
        public async Task ExecuteAsync_NumericOperation_InvalidNumericValues_ThrowsArgumentException()
        {
            // Arrange
            var name = $"Op_{Guid.NewGuid()}";
            var dateProviderMock = new Mock<IDateProvider>();
            dateProviderMock.SetupGet(d => d.UtcNow).Returns(DateTime.UtcNow);

            var operation = new Operation(name, "A + B", OperationType.Numeric);

            var repoMock = new Mock<IRepository<Operation>>();
            repoMock.Setup(r => r.GetByIdAsync(operation.Id)).ReturnsAsync(operation);

            var historyRepoMock = new Mock<IRepository<OperationHistory>>();

            var service = new OperationService(dateProviderMock.Object, repoMock.Object, historyRepoMock.Object);

            var request = new CalculateRequestDto(operation.Id, "not-a-number", "2");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecuteAsync(request));
            Assert.Equal("Invalid numeric values (Parameter 'request')", ex.Message);
            Assert.Equal("request", ex.ParamName);
        }
    }
}
