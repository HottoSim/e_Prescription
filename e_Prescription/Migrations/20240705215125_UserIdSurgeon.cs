using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class UserIdSurgeon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SurgeonId",
                table: "PatientBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_SurgeonId",
                table: "PatientBookings",
                column: "SurgeonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBookings_AspNetUsers_SurgeonId",
                table: "PatientBookings",
                column: "SurgeonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientBookings_AspNetUsers_SurgeonId",
                table: "PatientBookings");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_SurgeonId",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "SurgeonId",
                table: "PatientBookings");
        }
    }
}
