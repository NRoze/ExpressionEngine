using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressionEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTokensAndOperators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Operations");

            migrationBuilder.CreateTable(
                name: "Operator",
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
                    table.PrimaryKey("PK_Operator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "Expression", "Name" },
                values: new object[,]
                {
                    { 1, "A + B", "Add" },
                    { 2, "A - B", "Subtract" },
                    { 3, "A * B", "Multiply" },
                    { 4, "A / B", "Divide" },
                    { 5, "A % B", "Modulo" },
                    { 6, "min(A,B)", "Min" },
                    { 7, "max(A,B)", "Max" },
                    { 8, "A + B", "Concat" },
                    { 9, "A - B", "Remove" },
                    { 10, "A * B", "Repeat" }
                });

            migrationBuilder.InsertData(
                table: "Operator",
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
                table: "Token",
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
                    { 11, "max", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operator_Name",
                table: "Operator",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_Symbol",
                table: "Token",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operator");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
