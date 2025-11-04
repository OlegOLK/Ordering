using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.API.Migrations
{
    /// <inheritdoc />
    public partial class ErrorMessageForInboxEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "orders",
                table: "OutboxEvents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "orders",
                table: "OutboxEvents");
        }
    }
}
