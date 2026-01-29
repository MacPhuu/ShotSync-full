using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolBrackets_backend_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddHostIdToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "host_id",
                table: "events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_events_host_id",
                table: "events",
                column: "host_id");

            migrationBuilder.AddForeignKey(
                name: "FK_events_users_host_id",
                table: "events",
                column: "host_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_users_host_id",
                table: "events");

            migrationBuilder.DropIndex(
                name: "IX_events_host_id",
                table: "events");

            migrationBuilder.DropColumn(
                name: "host_id",
                table: "events");
        }
    }
}
