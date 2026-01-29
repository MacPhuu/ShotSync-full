using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolBrackets_backend_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddEventFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "entry_fee",
                table: "events",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "slogan",
                table: "events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "total_prize",
                table: "events",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "events");

            migrationBuilder.DropColumn(
                name: "entry_fee",
                table: "events");

            migrationBuilder.DropColumn(
                name: "format",
                table: "events");

            migrationBuilder.DropColumn(
                name: "slogan",
                table: "events");

            migrationBuilder.DropColumn(
                name: "total_prize",
                table: "events");
        }
    }
}
