using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "HealthcareProfessionals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "HealthcareProfessionals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HealthcareProfessionals_ApplicationUserId",
                table: "HealthcareProfessionals",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareProfessionals_AspNetUsers_ApplicationUserId",
                table: "HealthcareProfessionals",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareProfessionals_AspNetUsers_ApplicationUserId",
                table: "HealthcareProfessionals");

            migrationBuilder.DropIndex(
                name: "IX_HealthcareProfessionals_ApplicationUserId",
                table: "HealthcareProfessionals");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "HealthcareProfessionals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HealthcareProfessionals");
        }
    }
}
