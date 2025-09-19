using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InterviewBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductStock",
                columns: new[] { "Id", "Inventory", "IsOutOfStock", "ProductName" },
                values: new object[,]
                {
                    { 4, 10, false, "ProductA" },
                    { 5, 5, false, "ProductB" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductStock",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductStock",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
