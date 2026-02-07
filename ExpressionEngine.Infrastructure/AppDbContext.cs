using ExpressionEngine.Core.Models;
using ExpressionEngine.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpressionEngine.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Operation> Operations => Set<Operation>();
        public DbSet<Operator> Operators => Set<Operator>();
        public DbSet<Token> Tokens => Set<Token>();
        public DbSet<OperationHistory> OperationHistories => Set<OperationHistory>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Operation>().HasData(
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000001"), "Add", "A + B", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000002"), "Subtract", "A - B", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000003"), "Multiply", "A * B", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000004"), "Divide", "A / B", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000005"), "Modulo", "A % B", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000006"), "Min", "min(A,B)", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000007"), "Max", "max(A,B)", OperationType.Numeric),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000008"), "Concat", "A + B", OperationType.String),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-000000000009"), "Remove", "A - B", OperationType.String),
                new Operation(Guid.Parse("0180b8c0-0000-7e01-8000-00000000000a"), "Repeat", "A * B", OperationType.String)
            );

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Operator>().HasData(
                new Operator { Id = 1, Name = "Add", Symbol = "+", Category = OperatorType.NumericAndString },
                new Operator { Id = 2, Name = "Subtract", Symbol = "-", Category = OperatorType.NumericAndString },
                new Operator { Id = 3, Name = "Multiply", Symbol = "*", Category = OperatorType.NumericAndString },
                new Operator { Id = 4, Name = "Divide", Symbol = "/", Category = OperatorType.NumericOnly },
                new Operator { Id = 5, Name = "Modulo", Symbol = "%", Category = OperatorType.NumericOnly },
                new Operator { Id = 6, Name = "Min", Symbol = "min", Category = OperatorType.NumericOnly },
                new Operator { Id = 7, Name = "Max", Symbol = "max", Category = OperatorType.NumericOnly }
            );
            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasIndex(t => t.Symbol)
                    .IsUnique();
                entity.Property(t => t.Symbol)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<Token>().HasData(
                // Variables
                new Token { Id = 1, Symbol = "A", Type = TokenType.Variable },
                new Token { Id = 2, Symbol = "B", Type = TokenType.Variable },

                // Operators
                new Token { Id = 3, Symbol = "+", Type = TokenType.Operator },
                new Token { Id = 4, Symbol = "-", Type = TokenType.Operator },
                new Token { Id = 5, Symbol = "*", Type = TokenType.Operator },
                new Token { Id = 6, Symbol = "/", Type = TokenType.Operator },
                new Token { Id = 7, Symbol = "%", Type = TokenType.Operator },

                // Parentheses
                new Token { Id = 8, Symbol = "(", Type = TokenType.Parenthesis },
                new Token { Id = 9, Symbol = ")", Type = TokenType.Parenthesis },

                // Functions
                new Token { Id = 10, Symbol = "min", Type = TokenType.Function },
                new Token { Id = 11, Symbol = "max", Type = TokenType.Function },

                // Separators
                new Token { Id = 12, Symbol = ",", Type = TokenType.Separator }
            );

            modelBuilder.Entity<OperationHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
            });
        }
    }
}
