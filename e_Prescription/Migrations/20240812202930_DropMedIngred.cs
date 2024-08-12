using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class DropMedIngred : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications");

            migrationBuilder.AlterColumn<int>(
                name: "MedicationIngredientId",
                table: "PatientMedications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications",
                column: "MedicationIngredientId",
                principalTable: "MedicationIngredients",
                principalColumn: "MedicationIngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications");

            migrationBuilder.AlterColumn<int>(
                name: "MedicationIngredientId",
                table: "PatientMedications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedications_MedicationIngredients_MedicationIngredientId",
                table: "PatientMedications",
                column: "MedicationIngredientId",
                principalTable: "MedicationIngredients",
                principalColumn: "MedicationIngredientId");
        }
    }
}
