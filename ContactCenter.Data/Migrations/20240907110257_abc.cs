using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortSummary",
                table: "TicketAudit");

            migrationBuilder.RenameColumn(
                name: "DetailedDescription",
                table: "TicketAudit",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TicketAudit",
                newName: "DetailedDescription");

            migrationBuilder.AddColumn<string>(
                name: "ShortSummary",
                table: "TicketAudit",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
