using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class StrVit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighLimit",
                table: "Vital");

            migrationBuilder.DropColumn(
                name: "HighLimitDiastolic",
                table: "Vital");

            migrationBuilder.DropColumn(
                name: "LowLimit",
                table: "Vital");

            migrationBuilder.DropColumn(
                name: "LowLimitDiastolic",
                table: "Vital");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HighLimit",
                table: "Vital",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "HighLimitDiastolic",
                table: "Vital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LowLimit",
                table: "Vital",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "LowLimitDiastolic",
                table: "Vital",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
