using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class UserIdLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NurseId",
                table: "Admissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_NurseId",
                table: "Admissions",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_NurseId",
                table: "Admissions",
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
                name: "FK_Admissions_AspNetUsers_NurseId",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_NurseId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "Admissions");
        }
    }
}
