using ExpressionEngine.Core.Models;
using ExpressionEngine.Infrastructure.Data;
using ExpressionEngine.Infrastructure.Repositores;
using ExpressionEngine.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpressionEngine.UnitTests.Repositories
{
    public class EfRepositoryTests
    {
        private static AppDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Entity_And_Set_Id()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EfRepository<Operation>(context);

            var operation = new Operation
            {
                Name = "TestAdd",
                Expression = "A + B",
                OperationType = OperationType.Numeric
            };

            await repo.AddAsync(operation);

            Assert.True(operation.Id > 0);

            var fetched = await repo.GetByIdAsync(operation.Id);
            Assert.NotNull(fetched);
            Assert.Equal("TestAdd", fetched!.Name);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Entities()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EfRepository<Operation>(context);

            var op1 = new Operation { Name = "Op1", Expression = "A+B", OperationType = OperationType.Numeric };
            var op2 = new Operation { Name = "Op2", Expression = "A-B", OperationType = OperationType.Numeric };

            await repo.AddAsync(op1);
            await repo.AddAsync(op2);

            var all = await repo.GetAllAsync();

            Assert.Equal(2, all.Count);
            Assert.Contains(all, o => o.Name == "Op1");
            Assert.Contains(all, o => o.Name == "Op2");
        }

        [Fact]
        public async Task UpdateAsync_Should_Persist_Changes()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EfRepository<Operation>(context);

            var op = new Operation { Name = "Before", Expression = "A+B", OperationType = OperationType.Numeric };
            await repo.AddAsync(op);

            op.Name = "After";
            await repo.UpdateAsync(op);

            var fetched = await repo.GetByIdAsync(op.Id);
            Assert.NotNull(fetched);
            Assert.Equal("After", fetched!.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Entity()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EfRepository<Operation>(context);

            var op = new Operation { Name = "ToDelete", Expression = "A+B", OperationType = OperationType.Numeric };
            await repo.AddAsync(op);

            var before = await repo.GetAllAsync();
            Assert.Single(before);

            await repo.DeleteAsync(op.Id);

            var after = await repo.GetAllAsync();
            Assert.Empty(after);

            var fetched = await repo.GetByIdAsync(op.Id);
            Assert.Null(fetched);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_Should_NotThrow()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EfRepository<Operation>(context);

            // Should not throw when deleting a non-existing id
            await repo.DeleteAsync(9999);
        }
    }
}
