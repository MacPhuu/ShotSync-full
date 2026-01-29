using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolBrackets_backend_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "players",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_players_email",
                table: "players",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_players_user_id",
                table: "players",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_players_users_user_id",
                table: "players",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_users_user_id",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_email",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_user_id",
                table: "players");

            migrationBuilder.DropColumn(
                name: "email",
                table: "players");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "players");
        }
    }
}
