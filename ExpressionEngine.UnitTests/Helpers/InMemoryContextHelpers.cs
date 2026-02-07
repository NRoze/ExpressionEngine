using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpressionEngine.UnitTests.Helpers
{
    static internal class InMemoryContextHelpers
    {
        static public (Mock<IRepository<OperationHistory>> Repo, Capture<OperationHistory> Capture) CreateHistoryRepo()
        {
            var capture = new Capture<OperationHistory>();

            var historyRepoMock = new Mock<IRepository<OperationHistory>>();
            historyRepoMock
                .Setup(h => h.AddAsync(It.IsAny<OperationHistory>()))
                .Callback<OperationHistory>(h => capture.Value = h)
                .Returns(Task.CompletedTask);

            return (historyRepoMock, capture);
        }

        static public TestDbContext CreateInMemoryContext(IEnumerable<OperationHistory> histories)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TestDbContext(options);

            context.OperationHistories.AddRange(histories);
            context.SaveChanges();

            return context;
        }
        static public TestDbContext CreateInMemoryContext()
        {
            return CreateInMemoryContext(Array.Empty<OperationHistory>());
        }
    }
}
