using Microsoft.EntityFrameworkCore.Migrations;

namespace ConfirmationProject.Migrations
{
    public partial class vson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MailConfirmation",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailConfirmation",
                table: "Users");
        }
    }
}
