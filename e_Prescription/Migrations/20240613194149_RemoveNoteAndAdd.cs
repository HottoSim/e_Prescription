using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNoteAndAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Admissions");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PatientVitals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "PatientVitals");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
