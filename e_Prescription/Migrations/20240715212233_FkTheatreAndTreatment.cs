using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class FkTheatreAndTreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheatreId",
                table: "PatientBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentCodeId",
                table: "PatientBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Theatre",
                columns: table => new
                {
                    TheatreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theatre", x => x.TheatreId);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentCodes",
                columns: table => new
                {
                    TreatmentCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentCodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentCodeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentCodes", x => x.TreatmentCodeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_TheatreId",
                table: "PatientBookings",
                column: "TheatreId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_TreatmentCodeId",
                table: "PatientBookings",
                column: "TreatmentCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBookings_Theatre_TheatreId",
                table: "PatientBookings",
                column: "TheatreId",
                principalTable: "Theatre",
                principalColumn: "TheatreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBookings_TreatmentCodes_TreatmentCodeId",
                table: "PatientBookings",
                column: "TreatmentCodeId",
                principalTable: "TreatmentCodes",
                principalColumn: "TreatmentCodeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientBookings_Theatre_TheatreId",
                table: "PatientBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientBookings_TreatmentCodes_TreatmentCodeId",
                table: "PatientBookings");

            migrationBuilder.DropTable(
                name: "Theatre");

            migrationBuilder.DropTable(
                name: "TreatmentCodes");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_TheatreId",
                table: "PatientBookings");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_TreatmentCodeId",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "TheatreId",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "TreatmentCodeId",
                table: "PatientBookings");
        }
    }
}
