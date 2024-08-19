using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class MedIngreMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PharmacyMedicationIngredients",
                columns: table => new
                {
                    PharmacyMedicationIngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    PharmacyMedicationMedicationId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientId = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedicationIngredients", x => x.PharmacyMedicationIngredientId);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicationIngredients_ActiveIngredients_ActiveIngredientId",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicationIngredients_PharmacyMedications_PharmacyMedicationMedicationId",
                        column: x => x.PharmacyMedicationMedicationId,
                        principalTable: "PharmacyMedications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicationIngredients_ActiveIngredientId",
                table: "PharmacyMedicationIngredients",
                column: "ActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicationIngredients_PharmacyMedicationMedicationId",
                table: "PharmacyMedicationIngredients",
                column: "PharmacyMedicationMedicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PharmacyMedicationIngredients");
        }
    }
}
