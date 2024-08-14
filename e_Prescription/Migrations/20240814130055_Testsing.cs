using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class Testsing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_MedicationIngredients_MedicationId",
            //    table: "MedicationIngredients");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MedicationIngredients_MedicationId",
            //    table: "MedicationIngredients",
            //    column: "MedicationId",
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_MedicationIngredients_MedicationId",
            //    table: "MedicationIngredients");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MedicationIngredients_MedicationId",
            //    table: "MedicationIngredients",
            //    column: "MedicationId");
        }
    }
}
