using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class RemoveTicketsTablesMigartion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketHeading_TicketHeadingId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "TicketAudit_StatusId_fkey",
                table: "TicketAudit");

            migrationBuilder.DropTable(
                name: "TicketHeading");

            migrationBuilder.DropTable(
                name: "TicketStatus");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropIndex(
                name: "IX_TicketAudit_StatusId",
                table: "TicketAudit");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketHeadingId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketStatusId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketStatusId",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "TicketHeadingId",
                table: "Ticket",
                type: "integer",
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketStatus",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketStatus",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "TicketHeadingId",
                table: "Ticket",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "TicketStatusId",
                table: "Ticket",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketHeading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    HeadingName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHeading", x => x.Id);
                    table.ForeignKey(
                        name: "TicketHeading_TicketTypeId_fkey",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketAudit_StatusId",
                table: "TicketAudit",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketHeadingId",
                table: "Ticket",
                column: "TicketHeadingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketStatusId",
                table: "Ticket",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHeading_TicketTypeId",
                table: "TicketHeading",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketHeading_TicketHeadingId",
                table: "Ticket",
                column: "TicketHeadingId",
                principalTable: "TicketHeading",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket",
                column: "TicketStatusId",
                principalTable: "TicketStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "TicketAudit_StatusId_fkey",
                table: "TicketAudit",
                column: "StatusId",
                principalTable: "TicketStatus",
                principalColumn: "Id");
        }
    }
}
