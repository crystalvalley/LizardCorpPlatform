using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LizardCorpBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "guild",
                columns: table => new
                {
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guild", x => x.guild_id);
                });

            migrationBuilder.CreateTable(
                name: "guild_user",
                columns: table => new
                {
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guild_user", x => new { x.guild_id, x.user_id });
                });

            migrationBuilder.CreateTable(
                name: "minecraft_user",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    last_joined = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    playtime = table.Column<long>(type: "bigint", nullable: false),
                    is_playing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_minecraft_user", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "todo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    taskholder = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    complete_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    time_limit = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guild");

            migrationBuilder.DropTable(
                name: "guild_user");

            migrationBuilder.DropTable(
                name: "minecraft_user");

            migrationBuilder.DropTable(
                name: "todo");
        }
    }
}
