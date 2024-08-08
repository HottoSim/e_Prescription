using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ProvinceSuburb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Suburbs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Suburbs_ProvinceId",
                table: "Suburbs",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
               name: "FK_Suburbs_Provinces_ProvinceId",
             table: "Suburbs",
          column: "ProvinceId",
           principalTable: "Provinces",
    principalColumn: "ProvinceId",
    onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suburbs_Provinces_ProvinceId",
                table: "Suburbs");

            migrationBuilder.DropIndex(
                name: "IX_Suburbs_ProvinceId",
                table: "Suburbs");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Suburbs");
        }
    }
}
