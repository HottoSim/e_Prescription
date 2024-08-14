using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class AlertesModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContraIndications",
                columns: table => new
                {
                    IndicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChronicConditionId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContraIndications", x => x.IndicationId);
                    table.ForeignKey(
                        name: "FK_ContraIndications_ActiveIngredients_ActiveIngredientId",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContraIndications_ChronicConditions_ChronicConditionId",
                        column: x => x.ChronicConditionId,
                        principalTable: "ChronicConditions",
                        principalColumn: "ChronicCondotionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationInteractions",
                columns: table => new
                {
                    InterationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveIngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationInteractions", x => x.InterationId);
                    table.ForeignKey(
                        name: "FK_MedicationInteractions_ActiveIngredients_ActiveIngredientId",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ActiveIngredientId",
                table: "ContraIndications",
                column: "ActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContraIndications_ChronicConditionId",
                table: "ContraIndications",
                column: "ChronicConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationInteractions_ActiveIngredientId",
                table: "MedicationInteractions",
                column: "ActiveIngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContraIndications");

            migrationBuilder.DropTable(
                name: "MedicationInteractions");
        }
    }
}
