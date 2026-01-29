using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolBrackets_backend_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class InitialFullSchema_Production : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    venue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    number_of_players = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    is_displayed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_happen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    portrait = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    point = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<int>(type: "int", nullable: false),
                    nation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    first_player_id = table.Column<int>(type: "int", nullable: true),
                    second_player_id = table.Column<int>(type: "int", nullable: true),
                    table_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    first_player_point = table.Column<int>(type: "int", nullable: false),
                    second_player_point = table.Column<int>(type: "int", nullable: false),
                    race_to = table.Column<int>(type: "int", nullable: false),
                    is_start = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_finish = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    round_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    round_type = table.Column<int>(type: "int", nullable: false),
                    next_match_id_win = table.Column<int>(type: "int", nullable: true),
                    next_match_id_lose = table.Column<int>(type: "int", nullable: true),
                    next_match_position = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matches", x => x.id);
                    table.ForeignKey(
                        name: "FK_matches_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_matches_players_first_player_id",
                        column: x => x.first_player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_matches_players_second_player_id",
                        column: x => x.second_player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_histories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    match_id = table.Column<int>(type: "int", nullable: false),
                    result = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opponent_id = table.Column<int>(type: "int", nullable: false),
                    match_date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_histories_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_histories_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_in_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_in_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_player_in_events_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_in_events_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_matches_event_id",
                table: "matches",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_matches_first_player_id",
                table: "matches",
                column: "first_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_matches_second_player_id",
                table: "matches",
                column: "second_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_histories_event_id",
                table: "player_histories",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_histories_player_id",
                table: "player_histories",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_in_events_event_id",
                table: "player_in_events",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_in_events_player_id",
                table: "player_in_events",
                column: "player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matches");

            migrationBuilder.DropTable(
                name: "player_histories");

            migrationBuilder.DropTable(
                name: "player_in_events");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
