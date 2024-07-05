using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Discharges",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_ApplicationUserId",
                table: "Discharges",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_AspNetUsers_ApplicationUserId",
                table: "Discharges",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_AspNetUsers_ApplicationUserId",
                table: "Discharges");

            migrationBuilder.DropIndex(
                name: "IX_Discharges_ApplicationUserId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Discharges");
        }
    }
}
