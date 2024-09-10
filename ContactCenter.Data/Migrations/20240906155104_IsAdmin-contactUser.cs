using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactCenter.Data.Migrations
{
    public partial class IsAdmincontactUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "ContactUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "ContactUser");
        }
    }
}
