using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class CityColmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Suburbs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Suburbs_CityId",
                table: "Suburbs",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suburbs_Cities_CityId",
                table: "Suburbs",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburbs_Cities_CityId",
                table: "Suburbs");

            migrationBuilder.DropIndex(
                name: "IX_Suburbs_CityId",
                table: "Suburbs");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Suburbs");
        }
    }
}
