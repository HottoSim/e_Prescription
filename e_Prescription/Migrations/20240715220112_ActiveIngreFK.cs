using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ActiveIngreFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveIngredientId",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "Medications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_ActiveIngredientId",
                table: "Medications",
                column: "ActiveIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_ActiveIngredients_ActiveIngredientId",
                table: "Medications",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_ActiveIngredients_ActiveIngredientId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_ActiveIngredientId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "ActiveIngredientId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Medications");
        }
    }
}
