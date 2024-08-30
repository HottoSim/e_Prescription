using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class NurseIdMedGiven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NurseId",
                table: "MedicationsGiven",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationsGiven_NurseId",
                table: "MedicationsGiven",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationsGiven_AspNetUsers_NurseId",
                table: "MedicationsGiven",
                column: "NurseId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationsGiven_AspNetUsers_NurseId",
                table: "MedicationsGiven");

            migrationBuilder.DropIndex(
                name: "IX_MedicationsGiven_NurseId",
                table: "MedicationsGiven");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "MedicationsGiven");
        }
    }
}
