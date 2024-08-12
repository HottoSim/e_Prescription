using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ReMedClmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicationIngredientId",
                table: "PatientMedications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_MedicationIngredientId",
                table: "PatientMedications",
                column: "MedicationIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications",
                column: "MedicationIngredientId",
                principalTable: "MedicationIngredients",
                principalColumn: "MedicationIngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_MedicationIngredientId",
                table: "PatientMedications");

            migrationBuilder.DropColumn(
                name: "MedicationIngredientId",
                table: "PatientMedications");
        }
    }
}
