using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class DropTreatmentTheatre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theatre",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "TreatmentCode",
                table: "PatientBookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theatre",
                table: "PatientBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TreatmentCode",
                table: "PatientBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
