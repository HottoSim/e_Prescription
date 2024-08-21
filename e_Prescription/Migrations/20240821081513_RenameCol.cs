using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class RenameCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyMedicationIngredients_PharmacyMedications_PharmacyMedicationMedicationId",
                table: "PharmacyMedicationIngredients");

            migrationBuilder.RenameColumn(
                name: "MedicationId",
                table: "PharmacyMedications",
                newName: "PharmacyMedicationId");

            migrationBuilder.RenameColumn(
                name: "PharmacyMedicationMedicationId",
                table: "PharmacyMedicationIngredients",
                newName: "PharmacyMedicationId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyMedicationIngredients_PharmacyMedicationMedicationId",
                table: "PharmacyMedicationIngredients",
                newName: "IX_PharmacyMedicationIngredients_PharmacyMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyMedicationIngredients_PharmacyMedications_PharmacyMedicationId",
                table: "PharmacyMedicationIngredients",
                column: "PharmacyMedicationId",
                principalTable: "PharmacyMedications",
                principalColumn: "PharmacyMedicationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyMedicationIngredients_PharmacyMedications_PharmacyMedicationId",
                table: "PharmacyMedicationIngredients");

            migrationBuilder.RenameColumn(
                name: "PharmacyMedicationId",
                table: "PharmacyMedications",
                newName: "MedicationId");

            migrationBuilder.RenameColumn(
                name: "PharmacyMedicationId",
                table: "PharmacyMedicationIngredients",
                newName: "PharmacyMedicationMedicationId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyMedicationIngredients_PharmacyMedicationId",
                table: "PharmacyMedicationIngredients",
                newName: "IX_PharmacyMedicationIngredients_PharmacyMedicationMedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyMedicationIngredients_PharmacyMedications_PharmacyMedicationMedicationId",
                table: "PharmacyMedicationIngredients",
                column: "PharmacyMedicationMedicationId",
                principalTable: "PharmacyMedications",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
