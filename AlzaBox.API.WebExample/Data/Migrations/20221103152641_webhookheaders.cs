using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlzaBox.API.WebExample.Data.Migrations
{
    public partial class webhookheaders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "ChangeStatusRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestHeader",
                table: "ChangeStatusRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP",
                table: "ChangeStatusRequests");

            migrationBuilder.DropColumn(
                name: "RequestHeader",
                table: "ChangeStatusRequests");
        }
    }
}
