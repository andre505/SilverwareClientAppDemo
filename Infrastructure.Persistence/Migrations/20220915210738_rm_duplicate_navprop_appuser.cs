using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class rm_duplicate_navprop_appuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_ApplicationUserId1",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ApplicationUserId1",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ApplicationUserId1",
                table: "Clients",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_ApplicationUserId1",
                table: "Clients",
                column: "ApplicationUserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
