using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class DosageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosageForm",
                table: "Medications");

            migrationBuilder.AddColumn<int>(
                name: "DosageFormId",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DosageFormId",
                table: "Medications",
                column: "DosageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_DosageForms_DosageFormId",
                table: "Medications",
                column: "DosageFormId",
                principalTable: "DosageForms",
                principalColumn: "DosageFormId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_DosageForms_DosageFormId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_DosageFormId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "DosageFormId",
                table: "Medications");

            migrationBuilder.AddColumn<string>(
                name: "DosageForm",
                table: "Medications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
