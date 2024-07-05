using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class Clm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HealthcareProfessionalUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HealthcareProfessionalUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthcareProfessionalUserId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HealthcareProfessionalUserId",
                table: "AspNetUsers",
                column: "HealthcareProfessionalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "AspNetUsers",
                column: "HealthcareProfessionalUserId",
                principalTable: "HealthcareProfessionals",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
