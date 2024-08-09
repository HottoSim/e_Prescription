using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class ReDoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HighLimit",
                table: "Vital",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowLimit",
                table: "Vital",
                type: "float",
                nullable: true,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighLimit",
                table: "Vital");

            migrationBuilder.DropColumn(
                name: "LowLimit",
                table: "Vital");
        }
    }
}
