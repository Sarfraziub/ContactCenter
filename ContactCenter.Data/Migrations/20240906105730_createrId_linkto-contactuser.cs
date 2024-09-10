using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class createrId_linktocontactuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Call_ConversationId_fkey",
                table: "Call");

            migrationBuilder.DropForeignKey(
                name: "Ticket_AssigneeId_fkey",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "Ticket_CategoryId_fkey",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "Ticket_ContactId_fkey",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "Ticket_CreatorId_fkey",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "Ticket_LocationId_fkey",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_StatusId",
                table: "Ticket");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Ticket",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "''::character varying");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactId",
                table: "Ticket",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "Ticket",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AgentId",
                table: "Ticket",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "Call_ConversationId_fkey",
                table: "Call",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Agent_AgentId",
                table: "Ticket",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Contact_ContactId",
                table: "Ticket",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_ContactUser_CreatorId",
                table: "Ticket",
                column: "CreatorId",
                principalTable: "ContactUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Location_LocationId",
                table: "Ticket",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketCategory_CategoryId",
                table: "Ticket",
                column: "CategoryId",
                principalTable: "TicketCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_User_AssigneeId",
                table: "Ticket",
                column: "AssigneeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Call_ConversationId_fkey",
                table: "Call");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Agent_AgentId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Contact_ContactId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_ContactUser_CreatorId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Location_LocationId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketCategory_CategoryId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_User_AssigneeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_AgentId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Ticket");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Ticket",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "''::character varying",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Ticket",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactId",
                table: "Ticket",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StatusId",
                table: "Ticket",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "Call_ConversationId_fkey",
                table: "Call",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Ticket_AssigneeId_fkey",
                table: "Ticket",
                column: "AssigneeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Ticket_CategoryId_fkey",
                table: "Ticket",
                column: "CategoryId",
                principalTable: "TicketCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "Ticket_ContactId_fkey",
                table: "Ticket",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Ticket_CreatorId_fkey",
                table: "Ticket",
                column: "CreatorId",
                principalTable: "Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Ticket_LocationId_fkey",
                table: "Ticket",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
