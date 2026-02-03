using ExpressionEngine.Core.Enums;
using ExpressionEngine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpressionEngine.Infrastructure.Data
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
                new Operation { Id = 1, Name = "Add", Expression = "A + B" },
                new Operation { Id = 2, Name = "Subtract", Expression = "A - B" },
                new Operation { Id = 3, Name = "Multiply", Expression = "A * B" },
                new Operation { Id = 4, Name = "Divide", Expression = "A / B" },
                new Operation { Id = 5, Name = "Modulo", Expression = "A % B" },
                new Operation { Id = 6, Name = "Min", Expression = "min(A,B)" },
                new Operation { Id = 7, Name = "Max", Expression = "max(A,B)" },
                new Operation { Id = 8, Name = "Concat", Expression = "A + B" },
                new Operation { Id = 9, Name = "Remove", Expression = "A - B" },
                new Operation { Id = 10, Name = "Repeat", Expression = "A * B" }
            );

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Operator>().HasData(
                new Operator { Id = 1, Name = "Add", Symbol = "+", Category = OperationCategory.NumericAndString },
                new Operator { Id = 2, Name = "Subtract", Symbol = "-", Category = OperationCategory.NumericAndString },
                new Operator { Id = 3, Name = "Multiply", Symbol = "*", Category = OperationCategory.NumericAndString },
                new Operator { Id = 4, Name = "Divide", Symbol = "/", Category = OperationCategory.NumericOnly },
                new Operator { Id = 5, Name = "Modulo", Symbol = "%", Category = OperationCategory.NumericOnly },
                new Operator { Id = 6, Name = "Min", Symbol = "min", Category = OperationCategory.NumericOnly },
                new Operator { Id = 7, Name = "Max", Symbol = "max", Category = OperationCategory.NumericOnly }
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
                new Token { Id = 11, Symbol = "max", Type = TokenType.Function }
            );

            modelBuilder.Entity<OperationHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
            });
        }
    }
}
