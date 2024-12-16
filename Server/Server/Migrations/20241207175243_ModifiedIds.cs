using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "games",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "arenas",
                newName: "ArenaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "games",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ArenaId",
                table: "arenas",
                newName: "Id");
        }
    }
}
