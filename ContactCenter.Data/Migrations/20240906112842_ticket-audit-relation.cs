using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class ticketauditrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAudit_Ticket_TicketId",
                table: "TicketAudit");

            migrationBuilder.AddForeignKey(
                name: "TicketAudit_TicketId_fkey",
                table: "TicketAudit",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "TicketAudit_TicketId_fkey",
                table: "TicketAudit");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAudit_Ticket_TicketId",
                table: "TicketAudit",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
