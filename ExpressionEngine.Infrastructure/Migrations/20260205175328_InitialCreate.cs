using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressionEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "Expression", "Name", "OperationType" },
                values: new object[,]
                {
                    { new Guid("0180b8c0-0000-7e01-8000-000000000001"), "A + B", "Add", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000002"), "A - B", "Subtract", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000003"), "A * B", "Multiply", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000004"), "A / B", "Divide", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000005"), "A % B", "Modulo", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000006"), "min(A,B)", "Min", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000007"), "max(A,B)", "Max", 0 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000008"), "A + B", "Concat", 1 },
                    { new Guid("0180b8c0-0000-7e01-8000-000000000009"), "A - B", "Remove", 1 },
                    { new Guid("0180b8c0-0000-7e01-8000-00000000000a"), "A * B", "Repeat", 1 }
                });

            migrationBuilder.InsertData(
                table: "Operators",
                columns: new[] { "Id", "Category", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, 1, "Add", "+" },
                    { 2, 1, "Subtract", "-" },
                    { 3, 1, "Multiply", "*" },
                    { 4, 0, "Divide", "/" },
                    { 5, 0, "Modulo", "%" },
                    { 6, 0, "Min", "min" },
                    { 7, 0, "Max", "max" }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "Symbol", "Type" },
                values: new object[,]
                {
                    { 1, "A", 0 },
                    { 2, "B", 0 },
                    { 3, "+", 1 },
                    { 4, "-", 1 },
                    { 5, "*", 1 },
                    { 6, "/", 1 },
                    { 7, "%", 1 },
                    { 8, "(", 3 },
                    { 9, ")", 3 },
                    { 10, "min", 2 },
                    { 11, "max", 2 },
                    { 12, ",", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Name",
                table: "Operations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operators_Name",
                table: "Operators",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Symbol",
                table: "Tokens",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationHistories");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Tokens");
        }
    }
}
