using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ActiveIngredMed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicationIngredients",
                columns: table => new
                {
                    MedicationIngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientId = table.Column<int>(type: "int", nullable: false),
                    IngredientStrength = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationIngredients", x => x.MedicationIngredientId);
                    table.ForeignKey(
                        name: "FK_MedicationIngredients_ActiveIngredients_ActiveIngredientId",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationIngredients_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIngredients_ActiveIngredientId",
                table: "MedicationIngredients",
                column: "ActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIngredients_MedicationId",
                table: "MedicationIngredients",
                column: "MedicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationIngredients");
        }
    }
}
