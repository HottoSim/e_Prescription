using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConditionName",
                table: "ChronicConditions",
                newName: "ICD_10_CODE");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Suburbs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "ChronicConditions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Suburbs");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "ChronicConditions");

            migrationBuilder.RenameColumn(
                name: "ICD_10_CODE",
                table: "ChronicConditions",
                newName: "ConditionName");
        }
    }
}
