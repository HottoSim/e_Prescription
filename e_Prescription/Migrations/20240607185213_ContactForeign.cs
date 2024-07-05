using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ContactForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ContactDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuburbId",
                table: "ContactDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_CityId",
                table: "ContactDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_SuburbId",
                table: "ContactDetails",
                column: "SuburbId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactDetails_Cities_CityId",
                table: "ContactDetails",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.NoAction); // Change made here

            migrationBuilder.AddForeignKey(
                name: "FK_ContactDetails_Suburbs_SuburbId",
                table: "ContactDetails",
                column: "SuburbId",
                principalTable: "Suburbs",
                principalColumn: "SuburbId",
                onDelete: ReferentialAction.NoAction); // Change made here
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactDetails_Cities_CityId",
                table: "ContactDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactDetails_Suburbs_SuburbId",
                table: "ContactDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContactDetails_CityId",
                table: "ContactDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContactDetails_SuburbId",
                table: "ContactDetails");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ContactDetails");

            migrationBuilder.DropColumn(
                name: "SuburbId",
                table: "ContactDetails");
        }
    }
}
