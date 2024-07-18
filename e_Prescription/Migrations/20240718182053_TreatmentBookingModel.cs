using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class TreatmentBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientBookings_TreatmentCodes_TreatmentCodeId",
                table: "PatientBookings");

            migrationBuilder.DropIndex(
                name: "IX_PatientBookings_TreatmentCodeId",
                table: "PatientBookings");

            migrationBuilder.DropColumn(
                name: "TreatmentCodeId",
                table: "PatientBookings");

            migrationBuilder.CreateTable(
                name: "BookingTreatments",
                columns: table => new
                {
                    BookingTreatmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    TreatmentCodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTreatments", x => x.BookingTreatmentId);
                    table.ForeignKey(
                        name: "FK_BookingTreatments_PatientBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "PatientBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingTreatments_TreatmentCodes_TreatmentCodeId",
                        column: x => x.TreatmentCodeId,
                        principalTable: "TreatmentCodes",
                        principalColumn: "TreatmentCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingTreatments_BookingId",
                table: "BookingTreatments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTreatments_TreatmentCodeId",
                table: "BookingTreatments",
                column: "TreatmentCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingTreatments");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentCodeId",
                table: "PatientBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientBookings_TreatmentCodeId",
                table: "PatientBookings",
                column: "TreatmentCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBookings_TreatmentCodes_TreatmentCodeId",
                table: "PatientBookings",
                column: "TreatmentCodeId",
                principalTable: "TreatmentCodes",
                principalColumn: "TreatmentCodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
