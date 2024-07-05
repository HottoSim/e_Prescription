using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class AddLoggedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthcareProfessionalUserId",
                table: "Discharges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NurseId",
                table: "Discharges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_HealthcareProfessionalUserId",
                table: "Discharges",
                column: "HealthcareProfessionalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "Discharges",
                column: "HealthcareProfessionalUserId",
                principalTable: "HealthcareProfessionals",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropIndex(
                name: "IX_Discharges_HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "Discharges");
        }
    }
}
