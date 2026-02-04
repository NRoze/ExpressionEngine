using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressionEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommaToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "Symbol", "Type" },
                values: new object[] { 12, ",", 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
