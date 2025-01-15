using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableBookingSoccerField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoccerFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MinCapacity = table.Column<int>(type: "integer", nullable: false),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    Indoor = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoccerFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoccerFields_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldId = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_SoccerFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "SoccerFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingUser",
                columns: table => new
                {
                    RequestedBookingsId = table.Column<int>(type: "integer", nullable: false),
                    WaitingListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingUser", x => new { x.RequestedBookingsId, x.WaitingListId });
                    table.ForeignKey(
                        name: "FK_BookingUser_Bookings_RequestedBookingsId",
                        column: x => x.RequestedBookingsId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingUser_Users_WaitingListId",
                        column: x => x.WaitingListId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingUser1",
                columns: table => new
                {
                    ApprovedBookingsId = table.Column<int>(type: "integer", nullable: false),
                    ApprovedParticipantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingUser1", x => new { x.ApprovedBookingsId, x.ApprovedParticipantsId });
                    table.ForeignKey(
                        name: "FK_BookingUser1_Bookings_ApprovedBookingsId",
                        column: x => x.ApprovedBookingsId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingUser1_Users_ApprovedParticipantsId",
                        column: x => x.ApprovedParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatorId",
                table: "Bookings",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FieldId",
                table: "Bookings",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingUser_WaitingListId",
                table: "BookingUser",
                column: "WaitingListId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingUser1_ApprovedParticipantsId",
                table: "BookingUser1",
                column: "ApprovedParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_SoccerFields_OwnerId",
                table: "SoccerFields",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingUser");

            migrationBuilder.DropTable(
                name: "BookingUser1");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "SoccerFields");
        }
    }
}
