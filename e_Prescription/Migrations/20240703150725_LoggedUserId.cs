using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class LoggedUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NurseId",
                table: "Discharges",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_NurseId",
                table: "Discharges",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_AspNetUsers_NurseId",
                table: "Discharges",
                column: "NurseId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_AspNetUsers_NurseId",
                table: "Discharges");

            migrationBuilder.DropIndex(
                name: "IX_Discharges_NurseId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "Discharges");
        }
    }
}
