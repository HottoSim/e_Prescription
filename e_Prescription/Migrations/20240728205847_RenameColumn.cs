using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_PatientId",
                table: "PatientBookings");

            migrationBuilder.RenameColumn(
                name: "ChronicCondotionId",
                table: "PatientConditions",
                newName: "ChronicConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_MedicationId",
                table: "PatientMedications",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedications_PatientId",
                table: "PatientMedications",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_ChronicConditionId",
                table: "PatientConditions",
                column: "ChronicConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_PatientId",
                table: "PatientConditions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_PatientId",
                table: "PatientBookings",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_ActiveIngredientId",
                table: "PatientAllergies",
                column: "ActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_PatientId",
                table: "PatientAllergies",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_ActiveIngredients_ActiveIngredientId",
                table: "PatientAllergies",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditions_ChronicConditions_ChronicConditionId",
                table: "PatientConditions",
                column: "ChronicConditionId",
                principalTable: "ChronicConditions",
                principalColumn: "ChronicCondotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditions_Patients_PatientId",
                table: "PatientConditions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Medications_MedicationId",
                table: "PatientMedications",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "MedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_Patients_PatientId",
                table: "PatientMedications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_ActiveIngredients_ActiveIngredientId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditions_ChronicConditions_ChronicConditionId",
                table: "PatientConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditions_Patients_PatientId",
                table: "PatientConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Medications_MedicationId",
                table: "PatientMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_Patients_PatientId",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_MedicationId",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedications_PatientId",
                table: "PatientMedications");

            migrationBuilder.DropIndex(
                name: "IX_PatientConditions_ChronicConditionId",
                table: "PatientConditions");

            migrationBuilder.DropIndex(
                name: "IX_PatientConditions_PatientId",
                table: "PatientConditions");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_PatientId",
                table: "PatientBookings");

            migrationBuilder.DropIndex(
                name: "IX_PatientAllergies_ActiveIngredientId",
                table: "PatientAllergies");

            migrationBuilder.DropIndex(
                name: "IX_PatientAllergies_PatientId",
                table: "PatientAllergies");

            migrationBuilder.RenameColumn(
                name: "ChronicConditionId",
                table: "PatientConditions",
                newName: "ChronicCondotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_PatientId",
                table: "PatientBookings",
                column: "PatientId");
        }
    }
}
