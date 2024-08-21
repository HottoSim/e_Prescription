using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhamacyMedicationId",
                table: "PharmacyMedicationIngredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhamacyMedicationId",
                table: "PharmacyMedicationIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
