using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LizardCorpBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20240131 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_completed",
                table: "todo");

            migrationBuilder.AddColumn<decimal>(
                name: "confirmer",
                table: "todo",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmer",
                table: "todo");

            migrationBuilder.AddColumn<bool>(
                name: "is_completed",
                table: "todo",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
