using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class changeIncorretEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedItems",
                table: "OrderedItems");

            migrationBuilder.AddColumn<int>(
                name: "OrderedItemKey",
                table: "OrderedItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedItems",
                table: "OrderedItems",
                column: "OrderedItemKey");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_Id",
                table: "OrderedItems",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedItems",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_Id",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "OrderedItemKey",
                table: "OrderedItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedItems",
                table: "OrderedItems",
                column: "Id");
        }
    }
}
