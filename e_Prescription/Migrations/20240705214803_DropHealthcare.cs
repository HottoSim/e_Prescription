using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class DropHealthcare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientBookings_HealthcareProfessionals_HealthcareProfessionalId",
                table: "PatientBookings");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_HealthcareProfessionalId",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "HealthcareProfessionalId",
                table: "PatientBookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthcareProfessionalId",
                table: "PatientBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_HealthcareProfessionalId",
                table: "PatientBookings",
                column: "HealthcareProfessionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBookings_HealthcareProfessionals_HealthcareProfessionalId",
                table: "PatientBookings",
                column: "HealthcareProfessionalId",
                principalTable: "HealthcareProfessionals",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
